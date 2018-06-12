
public class Board {
	
	public String[] tiles = new String[9];

	public Board() {
		for (int i = 0; i < tiles.length; i++) {
			tiles[i] = "-";
		}
	}
	
	/** Prints the board to the console */
	public void refreshBoard() {
		System.out.println(" " + tiles[0] + " | " + tiles[1] + " | " + tiles[2]);
		System.out.println("---|---|---");
		System.out.println(" " + tiles[3] + " | " + tiles[4] + " | " + tiles[5]);
		System.out.println("---|---|---");
		System.out.println(" " + tiles[6] + " | " + tiles[7] + " | " + tiles[8] + "\n");
	}
	
	/** Decides if the board is full (without empty spaces
    @return  True if the board is full/no more moves to make */
	public boolean isFull() {
		for (int i = 0; i < tiles.length; i++) {
			if (tiles[i].equals("-"))
				return false;
		}
		return true;
	}
	
	/** Makes a move on the board.
    @param player  A string of who is up: X or O. 
    	   position  An int of the position on the board where it should go 
    @return  void */
	public void makeMove(String player, int position) {
		tiles[position] = player;
	}
	
	/** converts the board into a string
    @return  String representation of the board. */
	public String boardToString() {
		String returnString = "";
		
		for (int i = 0; i < tiles.length; i++) {
			returnString += tiles[i];
		}
		return returnString;
	}
	
	/** Decides if there is a winner.
    @return  True if there is a winner. */
	public boolean isWinner() {
		if (tiles[0].equals(tiles[1]) && tiles[1].equals(tiles[2]) && !tiles[0].equals("-")) {
			return true;
		} else if (tiles[3].equals(tiles[4]) && tiles[4].equals(tiles[5]) && !tiles[3].equals("-")) {
			return true;
		} else if (tiles[6].equals(tiles[7]) && tiles[7].equals(tiles[8]) && !tiles[6].equals("-")) {
			return true;
		} else if (tiles[0].equals(tiles[3]) && tiles[3].equals(tiles[6]) && !tiles[0].equals("-")) {
			return true;
		} else if (tiles[1].equals(tiles[4]) && tiles[4].equals(tiles[7]) && !tiles[1].equals("-")) {
			return true;
		} else if (tiles[2].equals(tiles[5]) && tiles[5].equals(tiles[8]) && !tiles[2].equals("-")) {
			return true;
		} else if (tiles[0].equals(tiles[4]) && tiles[4].equals(tiles[8]) && !tiles[0].equals("-")) {
			return true;
		} else if (tiles[2].equals(tiles[4]) && tiles[4].equals(tiles[6]) && !tiles[2].equals("-")) {
			return true;
		} else {
			return false;
		}
	}
	
	/** Displays who is the winner if there is one.
    @return  String of the player's game piece who won. */
	public String displayWinner() {
		if (tiles[0].equals(tiles[1]) && tiles[1].equals(tiles[2])) {
			return tiles[0];
		} else if (tiles[3].equals(tiles[4]) && tiles[4].equals(tiles[5])) {
			return tiles[3];
		} else if (tiles[6].equals(tiles[7]) && tiles[7].equals(tiles[8])) {
			return tiles[6];
		} else if (tiles[0].equals(tiles[3]) && tiles[3].equals(tiles[6])) {
			return tiles[0];
		} else if (tiles[1].equals(tiles[4]) && tiles[4].equals(tiles[7])) {
			return tiles[1];
		} else if (tiles[2].equals(tiles[5]) && tiles[5].equals(tiles[8])) {
			return tiles[2];
		} else if (tiles[0].equals(tiles[4]) && tiles[4].equals(tiles[8])) {
			return tiles[0];
		} else if (tiles[2].equals(tiles[4]) && tiles[4].equals(tiles[6])) {
			return tiles[2];
		} else {
			return "null";
		}
	}
	
	
}
