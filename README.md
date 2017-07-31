# NickVelascoExperianProject
Simple program to do the following outlined below:


The included json file contains a list of records with a name, id, and an array of tags.

Please write a program (in either C# or javascript) that produces a list of each pair of names which have 2 or more tags in common, in the format "name1, name2|name1, name2|..."

For example:

	Jana Stevenson and Sylvia Norman have the following tags respectively:

	campaign, shopping, buyer
	shopping, buyer, clicker
	      
	Because they both have a  buyer and shopping tag, Jana Stevenson, Holmes Stevens should be added to the list.

	Pearson Marquez and Fern Wise have the following tags respectively

	shopping, non-clicker
	promo, clicker, non-clicker

	Because they only share one tag "non-clicker" the pair "Pearson Marquez, Fern Wise" should not appear on the list.


Acceptance Criteria
 
 The output should be printed to the console.

 Each pair of names should only appear once in the list, the order does not matter.
 
 The json can be included as string or object in the file or loaded externally.

 You may assume all tags are lowercase and are distinct per user (promo may only appear once in a list)
 
 

