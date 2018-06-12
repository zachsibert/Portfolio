public interface SetInterface {
	/**
	 * Adds a new entry to this set, avoiding duplicates.
	 *
	 * @param newEntry
	 *            The integer to be added as a new entry.
	 * @return True if the addition is successful, or false if the item already
	 *         is in the set.
	 */
	public boolean add(int amount, Word newValue);

	/**
	 * Tests whether this set contains a given entry.
	 *
	 * @param anEntry
	 *            The entry to locate.
	 * @return True if the set contains anEntry, or false if not.
	 */
	public int index(Word anEntry);

	/**
	 * Retrieves all entries that are in this set.
	 *
	 * @return A newly allocated array of all the entries in the set, where the
	 *         size of the array is equal to the number of entries in the set.
	 */
	public Word[] toArray();
	
	
	/**
	 * Retrieves the count of occurances of a given word.
	 *
	 * @return An int representation of the number of occurances.
	 */
	public int getCount(Word anEntry);
	
	/**
	 * Sorts the array using bubble sort
	 */
	public void bubbleSort();
	
	/**
	 * Retrieves the rank in terms of count amongst all other words 
	 *
	 * @return An String of the word that ocurrs 
	 */
	public String lookupRank(int n);
} // end SetInterface