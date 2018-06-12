import java.util.Scanner;

public class TicTacToeInteraction {
	public static void main(String[] args) {
		Scanner in = new Scanner(System.in);
		Board b = new Board();
		int turnCount = 1;
		
		System.out.println("(Please allow time for the game to load ~30 seconds)");
		TicTacToe t = new TicTacToe();
		System.out.println("Welcome to Tic Tac Toe. Please enter a number between 0-8 to make your move:");
		
		b.refreshBoard();
		
		while (!b.isWinner() || !b.isFull()) {
			if (b.isWinner() || b.isFull()) {
				break;
			}
			if (turnCount % 2 != 0) {
				System.out.println("Your turn:");
				int boardIndex = in.nextInt();
				b.makeMove("X", boardIndex);
			} else {
				System.out.println("Computer's turn:");
				b.makeMove("O", t.getBestMove(b.boardToString()));
			}
			turnCount++;
			b.refreshBoard();
		}
		
		if (b.isWinner()) {
			System.out.println(b.displayWinner() + " Wins!");
		} else {
			System.out.println("Tie!");
		}
		in.close();
		
		
	}
}
