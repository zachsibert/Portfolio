import java.util.Arrays;

public class ResizableArraySet {

	public boolean isSorted;
	public int length, numElements;
	public Word[] elements;
	
	public ResizableArraySet(int length, int numElements, boolean isSorted) {
		this.length = length;
		this.elements = new Word[length];
	}
	
	public ResizableArraySet() {
		this(10000, 0, false);
	}
	
	// Helper method for add(). This checks if the array is fully 
	// populated with int elements != 0. 0 is the default value for a new int array
	// and thus will be treated as "empty" in this program
	
	// COMBINE isFull WITH getSize TO MAKE IT MORE EFFICIENT. WHEN YOU FIND A NULL VALUE, RETURN FALSE, ELSE RETURN TRUE AT THE END OF THE LOOP 
	private boolean isFull() {
		return numElements == length;
	}

	public boolean addToArray(int amount, Word newValue) {
		int index = index(newValue);
		if (index != -1) {
			elements[index].count += amount;
			return true; 
		} else if (isFull()) { // CHECK IF THIS WORKS WITH ADDING AFTER IT'S FULL
			int insertIndex = elements.length;
			elements = Arrays.copyOf(elements, length*2);
			length = elements.length;
			elements[insertIndex] = newValue;
			numElements += 1;
			//sort();
			isSorted = false;
			return true;
		} else {
			elements[numElements] = newValue;
			numElements += 1;
			isSorted = false;
			return true;
		}
	}

	public int index(Word anEntry) {
		for (int i = 0; i < elements.length; i++) {
			if (elements[i] != null && elements[i].word.equals(anEntry.word)) {
				return i;
			}
		}
		return -1;
	}
	
	public Word[] toArray() {
		int counter = 0;
		int returnArrayLength = numElements;
		Word[] returnArray = new Word[returnArrayLength];
		
		for (int i = 0; i < elements.length; i++) {
			if (elements[i] != null) {
				 returnArray[counter] = elements[i];
				 counter++;
			}
		}
		return returnArray;
	}
	
	public int getCount(Word anEntry) {
		int index = index(anEntry);
		if (index != -1)
			return elements[index(anEntry)].count;
		else
			return 0;
	}
	
	public String lookupRank(int n) {
		if (!isSorted) {
			bubbleSort();
		}
		return elements[n].word;
	}
	
	public void bubbleSort() {
		int n = numElements;  
	    Word temp;
	    
		for (int i=0; i < n; i++){  
            for (int j=1; j < (n-i); j++) {  
                     if ((elements[j-1].count < elements[j].count) 
                    		 || ((elements[j-1].word.compareTo(elements[j].word) > 0) && elements[j-1].count == elements[j].count)) {  
                            //swap elements  
                            temp = elements[j-1];  
                            elements[j-1] = elements[j];  
                            elements[j] = temp;  
                   }          
            }
		}
	}
}