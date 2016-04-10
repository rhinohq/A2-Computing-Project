[ExID]
7

[ExTitle]
Introducing While Loops

[ExDescription]
In programming, we often want to run the same code over and over until something happens. For this we use 'loops'. These are structures that run code multiple times until a condition is met. Once such type of loop is a 'while loop'. They check the condition before running the code each time. In the editor, is an example of while loop. Change the condition of the while statement to only run the code if num is less than 10.

[ExCodeTemplate]
num = 0/r/rwhile condition do -- Change the condition so the code only runs when num is less than 10/r/tprint(string.format("The number = {0}", num))/r/tnum = num + 1/rend

[ExAppendCode]

[ExMarkScheme]
<?xml version="1.0" encoding="utf-8"?>
<MarkScheme>
	<ControlStructures>
		<ControlStructure StructType="WHILE">num &#60; 10</ControlStructure>
	</ControlStructures>
</MarkScheme>