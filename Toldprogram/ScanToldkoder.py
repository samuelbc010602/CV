from bs4 import BeautifulSoup as scrapper
import pandas as pd
from selenium import webdriver
from selenium.webdriver.common.by import By
from collections import defaultdict
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
import concurrent.futures
import threading
import sys
import math
import os

print("Press enter when you have entered your input!")
dateOfSearch = int(input("Write the current date in the following format YYYY/MM/DD, fx: 20220121 "))
nameOfCodes = str(input("Write the name of the html file with the different product codes, fx: produktKoder.html "))
outputFileCSV = str(input("Write the name of the CSV file to write the data to, fx tariff.csv "))
countryFrom = str(input("Write the abbreviation of the country that you want to test, fx IN for India "))
with open("{}".format(nameOfCodes)) as fp:
    soup = scrapper(fp, "lxml")
productCode = []
for row in soup.tbody:
    productCode.append(row.td.get_text())
tariffData = defaultdict(list)
linksToCheck = []
num_workers = min(32, os.cpu_count() + 4)-1

def getTariffInfo(codesToSearch,start,end):
    driver = webdriver.Chrome()
    for count,x in enumerate(codesToSearch[start:end]):
        x = x.replace(" ","")
        if len(tariffData[x]) > 0:
            if tariffData[x][0] == "Cancelled":
                continue
        driver.get("https://ec.europa.eu/taxation_customs/dds2/taric/measures.jsp?Lang=en&SimDate={}&Area={}&MeasType=&StartPub=&EndPub=&MeasText=&GoodsText=&op=&Taric={}&search_text=goods&textSearch=&LangDescr=en&OrderNum=&Regulation=&measStartDat=&measEndDat=".format(dateOfSearch,countryFrom,x))
        soup = scrapper(driver.page_source,"html5lib")
        container = soup.find_all("tr",{"valign":"baseline"})
        rateDescription = defaultdict(str)
        fields = soup.find_all("tr",{"valign":"baseline"})
        
        if len(fields) == 0:
            links = soup.select(".blue_link nobr")
            links = [y.get_text() for y in links]
            for y in links:
                linksToCheck.append(y.replace("\n",""))
            continue
        else:
            chapter = soup.find_all("td",{"class":"chaplabel"})[2].findChildren("nobr",recursive=False)[0].get_text().split(" ")[1]
            description = soup.select("#section{} nobr".format(chapter))[0].get_text()+"; "+soup.select("#section{} .to_highlight".format(chapter))[0].get_text()+" "+soup.select(".measureSelectedNode .to_highlight")[0].get_text()
            
        for y in fields:
            try:
                rateDescription[y.find_all("td")[0].get_text().replace("\n","").replace(" ","")[:16]] = y.find_all("td")[1].findChildren("span",{"class":"duty_rate"})[0].get_text().replace("%","")
            except:
                continue
        try:
            tariffData[x].append(float(rateDescription["Thirdcountryduty"]))
            tariffData[x].append(float(rateDescription["Tariffpreference"]))
            tariffData[x].append(description)
            tariffData[x].append(driver.current_url)
            tariffData[x].append(round(float(rateDescription["Thirdcountryduty"])-float(rateDescription["Tariffpreference"]),3))
        except:
            continue
    driver.close()

def getData(listToSearch):
    with concurrent.futures.ThreadPoolExecutor(num_workers) as executor:
        amountOfPro = int(math.floor(len(listToSearch)/num_workers))
        intervals = [1]
        for x in range(int(math.floor((len(listToSearch)-1)/amountOfPro))):
            intervals.append((x+1)*amountOfPro)
        intervals[-1] = len(listToSearch)-1
        first,second = 0,1
        threads = []
        for _ in range(int(math.floor((len(listToSearch)-1)/amountOfPro))):
            threads.append(executor.submit(getTariffInfo,listToSearch,intervals[first],intervals[second]))
            first = first+1
            second = second+1
        for waiting in concurrent.futures.as_completed(threads):
            print()
        
if __name__ == '__main__':
    getData(productCode)
    getData(linksToCheck)
    
    for x in tariffData.keys():
        if len(tariffData[x]) == 0:
            tariffData[x].append(0.0)       
    tariffSorted = sorted(tariffData.keys(),key=lambda x:tariffData[x][-1],reverse=True)
    productNumber,firstRate,secondRate,description,links,diff = [],[],[],[],[],[]
    for count,x in enumerate(tariffSorted):
        if tariffData[x][0] == 0.0:
            break
        elif len(tariffData[x]) < 5:
            continue
        else:
            productNumber.append(x)
            firstRate.append(tariffData[x][0])
            secondRate.append(tariffData[x][1])
            description.append(tariffData[x][2])
            links.append(tariffData[x][3])
            diff.append(tariffData[x][4])
            
    collectionOfList = {"Product Code":productNumber,"Third Country Duty":firstRate,"GSP":secondRate,"Description":description,"Difference":diff,"Link":links}
    df = pd.DataFrame(collectionOfList)
    print(df)
    df.to_csv("".format(outputFileCSV),index=False)
        
