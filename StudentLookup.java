/**
 * Your implementation of the LookupInterface.  The only public methods
 * in this class should be the ones that implement the interface.  You
 * should write as many other private methods as needed.  Of course, you
 * should also have a public constructor.
 * 
 * @author Zach Sibert
 */
  
public class StudentLookup implements LookupInterface {
	
	ResizableArraySet set;
	
	public StudentLookup() {
		set = new ResizableArraySet();
	}

	@Override
	public void addString(int amount, String s) {
		set.addToArray(amount, new Word(s, amount)); 
	}

	@Override
	public int lookupCount(String s) {
		Word tmp = new Word(s, 0);
		return set.getCount(tmp);
	}

	@Override
	public String lookupPopularity(int n) {
		return set.lookupRank(n);
	}

	@Override
	public int numEntries() {
		return set.numElements;
	}
    
}
