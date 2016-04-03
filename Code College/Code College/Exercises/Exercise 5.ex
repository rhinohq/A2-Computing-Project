[ExID]
5

[ExTitle]
Relational Operators

[ExDescription]
In programming, we want to be able to make the computer do different things depending on certain situations. e.g. Make a page redirect if a person's age is lower than 18. 

To make this happen, we use if statements with relational operators. These are things that check how an object compares to another. The operators are as follows: equals to, ==, not equal to, ~=, less than, <, greater than, >, less than or equal to, <=, and greater than or equal to, >=.

Create an if statement below to check whether age is less than or equal to 18.

[ExCodeTemplate]
age = math.random(1, 17) -- Creates a random number between 1 and 17\r\rprint(age)\rif condition then\r\tprint("You are not old enough, go home.")\rend

[ExAppendCode]

[ExMarkScheme]
<?xml version="1.0" encoding="utf-8"?>
<MarkScheme>
	<Output>You are not old enough, go home.</Output>

	<ControlStructures>
		<ControlStructure StructType="IF">age &#60;= 18</ControlStructure>
	</ControlStructures>
</MarkScheme>