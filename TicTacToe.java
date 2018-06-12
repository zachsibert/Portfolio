import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Set;

/**
 * @author ZachSibert
 * CSE 274 -- Norm Krumpe. Project 6
 *
 */
public class TicTacToe {

	public ArrayDictionary<String, Integer> boardsDictionary;
	public ArrayList<String> boardsList;
	
	public TicTacToe() {
		boardsDictionary = new ArrayDictionary<String, Integer>();
		boardsList = new ArrayList<String>();
		generateBoards();
		calculateBestMove();
	}
	
	/** generates all possible board combinations and loads them into an arraylist.
    @param void */
	public void generateBoards() {
		boardsList.add("---------");
		String[] c1 = {"X", "-", "-", "-", "-", "-", "-", "-", "-"};
		String[] c2 = {"X", "O", "-", "-", "-", "-", "-", "-", "-"};
		String[] c3 = {"X", "X", "O", "-", "-", "-", "-", "-", "-"};
		String[] c4 = {"X", "X", "O", "O", "-", "-", "-", "-", "-"};
		String[] c5 = {"X", "X", "X", "O", "O", "-", "-", "-", "-"};
		String[] c6 = {"X", "X", "X", "O", "O", "O", "-", "-", "-"};
		String[] c7 = {"X", "X", "X", "X", "O", "O", "O", "-", "-"};
		String[] c8 = {"X", "X", "X", "X", "O", "O", "O", "O", "-"};
		
		permute(c1, 0);
		permute(c2, 0);
		permute(c3, 0);
		permute(c4, 0);
		permute(c5, 0);
		permute(c6, 0);
		permute(c7, 0);
		permute(c8, 0);
	}
	
	
	// Converts String arrays to String format
	/** Converts String arrays to String format
    @param s A string array
    @return  The array converted into a string without spaces */
	private String arrayToString(String[] s) {
		String returnString = "";
		
		for (String str : s) {
			returnString += str;
		}
		return returnString;
	}
	
	/** Sees whether a specific entry is in this dictionary.
    @param a A String array of values.
    @return  Void */
	public void permute(String[] a, int k) {
		String[] tmp = a;
		if (k == a.length) {
            for (int i = 0; i < a.length; i++) {
            	tmp[i] = a[i];
            }
            String addString = arrayToString(tmp);
            if (!boardsList.contains(addString)) {
            	boardsList.add(addString);
            }
        } else {
            for (int i = k; i < a.length; i++) {
                String temp = a[k];
                a[k] = a[i];
                a[i] = temp;
 
                permute(a, k + 1);
 
                temp = a[k];
                a[k] = a[i];
                a[i] = temp;
            }
        }
    }

	/** Calculates the best move for each board in the arraylist. Then loads them into the dictionary.
    @param none.
    @return void */
	public void calculateBestMove() {
		for (int i = 0; i < boardsList.size(); i++) {
			String board = boardsList.get(i);
			int xCount = 0, emptyCount = 0;
			for (int k = 0; k < board.length(); k++) {
				if (board.substring(k, k+1).equals("X")) {
					++xCount;
				} else if (board.substring(k, k+1).equals("-")) {
					emptyCount++;
				}
			}	
			if (xCount == 1) { // in this case, nobody can win yet. place in center if the user's first turn wasn't there.
				if (!board.substring(4, 5).equals("-"))
					boardsDictionary.add(board, 0);
				else {
					boardsDictionary.add(board, 4);
				}	
			} else if (emptyCount == 1) { // if the board is one but full
				for (int k = 0; i < board.length(); i++) {
					if (board.substring(k, k+1).equals("-")) {
						boardsDictionary.add(board, k);
					}
				}
			} else {
				dictionaryHelper(board);
				boolean tmp = dictionaryHelper(board);
				
				if (tmp == false) {
					for (int k = 0; i < board.length(); i++) {
						if (board.substring(k, k+1).equals("-"))
							boardsDictionary.add(board, k);
					}
				}
			}				
			}
		}

	
	/** A helper method for calculateBestMove()
    @param board  A string representation of the board
    @return  True if the board was successfully loaded into the dictionary based on
    the board's situation */
	private boolean dictionaryHelper(String board) {
		
		// row 1
		if (board.substring(0,1).equals(board.substring(1,2)) && board.substring(2,3).equals("-")) {
			boardsDictionary.add(board, 2);
			return true;
		} else if (board.substring(0,1).equals(board.substring(2,3)) && board.substring(1,2).equals("-")) {
			boardsDictionary.add(board, 1);
			return true;
		} else if (board.substring(1,2).equals(board.substring(2,3)) && board.substring(0,1).equals("-")) {
			boardsDictionary.add(board, 0);
			return true;
		}
		
		// row 2
		if (board.substring(3,4).equals(board.substring(4,5)) && board.substring(5,6).equals("-")) {
			boardsDictionary.add(board, 5);
			return true;
		} else if (board.substring(3,4).equals(board.substring(5,6)) && board.substring(4,5).equals("-")) {
			boardsDictionary.add(board, 4);
			return true;
		} else if (board.substring(4,5).equals(board.substring(5,6)) && board.substring(3,4).equals("-")) {
			boardsDictionary.add(board, 3);
			return true;
		}
		
		// row 3
		if (board.substring(6,7).equals(board.substring(7,8)) && board.substring(8,9).equals("-")) {
			boardsDictionary.add(board, 8);
			return true;
		} else if (board.substring(6,7).equals(board.substring(8,9)) && board.substring(7,8).equals("-")) {
			boardsDictionary.add(board, 7);
			return true;
		} else if (board.substring(7,8).equals(board.substring(8,9)) && board.substring(6,7).equals("-")) {
			boardsDictionary.add(board, 6);
			return true;
		}
		
		// column 1
		if (board.substring(0,1).equals(board.substring(3,4)) && board.substring(6,7).equals("-")) {
			boardsDictionary.add(board, 6);
			return true;
		} else if (board.substring(0,1).equals(board.substring(6,7)) && board.substring(3,4).equals("-")) {
			boardsDictionary.add(board, 3);
			return true;
		} else if (board.substring(3,4).equals(board.substring(6,7)) && board.substring(0,1).equals("-")) {
			boardsDictionary.add(board, 0);
			return true;
		}
		
		// column 2
		if (board.substring(1,2).equals(board.substring(4,5)) && board.substring(7,8).equals("-")) {
			boardsDictionary.add(board, 7);
			return true;
		} else if (board.substring(1,2).equals(board.substring(7,8)) && board.substring(4,5).equals("-")) {
			boardsDictionary.add(board, 4);
			return true;
		} else if (board.substring(4,5).equals(board.substring(7,8)) && board.substring(1,2).equals("-")) {
			boardsDictionary.add(board, 1);
			return true;
		}
		
		// column 3
		if (board.substring(2,3).equals(board.substring(5,6)) && board.substring(8,9).equals("-")) {
			boardsDictionary.add(board, 8);
			return true;
		} else if (board.substring(2,3).equals(board.substring(8,9)) && board.substring(5,6).equals("-")) {
			boardsDictionary.add(board, 5);
			return true;
		} else if (board.substring(5,6).equals(board.substring(8,9)) && board.substring(2,3).equals("-")) {
			boardsDictionary.add(board, 2);
			return true;
		}
		
		// diagonal from top left to bottom right
		if (board.substring(0,1).equals(board.substring(4,5)) && board.substring(8,9).equals("-")) {
			boardsDictionary.add(board, 8);
			return true;
		} else if (board.substring(0,1).equals(board.substring(8,9)) && board.substring(4,5).equals("-")) {
			boardsDictionary.add(board, 4);
			return true;
		} else if (board.substring(4,5).equals(board.substring(8,9)) && board.substring(0,1).equals("-")) {
			boardsDictionary.add(board, 0);
			return true;
		}
		
		// diagonal from top right to bottom left
		if (board.substring(2,3).equals(board.substring(4,5)) && board.substring(6,7).equals("-")) {
			boardsDictionary.add(board, 6);
			return true;
		} else if (board.substring(2,3).equals(board.substring(6,7)) && board.substring(4,5).equals("-")) {
			boardsDictionary.add(board, 4);
			return true;
		} else if (board.substring(4,5).equals(board.substring(6,7)) && board.substring(2,3).equals("-")) {
			boardsDictionary.add(board, 2);
			return true;
		}
		return false;
	}
	
	/** Returns the best move for a given board in the dictionary
    @param board  An string representation of the board
    @return  an int value that represents where the best move is for the given board */
	public int getBestMove(String board) {
		return boardsDictionary.getValue(board);
	}

	// =========== Getters =============
	public ArrayList<String> getBoardsList() {
		return boardsList;
	}


	public void setBoardsList(ArrayList<String> boardsList) {
		this.boardsList = boardsList;
	}

	
	
	
}
