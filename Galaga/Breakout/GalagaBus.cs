using DIKUArcade.Events;
namespace GetGameBus {
public static class getGameBus {
    private static GameEventBus eventBus;
    public static GameEventBus GetBus() {
            return getGameBus.eventBus ?? (getGameBus.eventBus = new GameEventBus());
        }
    }    
}