
public enum GAME_PHASE : byte { PLANNING, PLAYING }

public static class GamePhase {
	
	private	static	GAME_PHASE	iActuaPahse = GAME_PHASE.PLANNING;

	public static void Switch() {
		
		if ( iActuaPahse == GAME_PHASE.PLAYING ) return;
		
		iActuaPahse = GAME_PHASE.PLAYING;
		
	}
	
	public static  GAME_PHASE GetCurrent() {
		return iActuaPahse;
	}
	
}