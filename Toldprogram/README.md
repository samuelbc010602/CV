# Customs program
This was a program I made in continuation of my father's work. I was given the task of creating a program that could retrieve all customs codes
from a particular country and compare them with the customs codes of other countries and then rank them in relation to the items where there was the biggest difference, in other words
find the goods where a certain country had to pay the least in customs to sell it on the Danish market compared to other countries.

The program itself uses parallelization to read customs codes simultaneously to reduce the running time of the program itself. By parallelizing the task, you could
do it much faster than if you had just used a core to read all thousands of customs codes sequentially.
Besides this I also use webscraping to read data from the internet, note that this program requires a web driver to be installed before you can run the program.
