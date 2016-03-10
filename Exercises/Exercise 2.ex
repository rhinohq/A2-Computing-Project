[ExID]
2

[ExTitle]
Variables

[ExDescription]
In programming, we store data and values in variables for easy access. Variables are places in memory than can be read and written to.

For Example, I can store my name in a 'name' variable like so:
name = "Bob"

Then I can store my age in another variable like this:
age = 100

Create your own variables called 'name' and 'age'. Store your name and age in the variables you create.

[ExCodeTemplate]

[ExAppendCode]
print(string.format("name = {0}", name))
print(string.format("age = {0}", age))

[ExMarkScheme]
<?xml version="1.0" encoding="utf-8"?>
<MarkScheme>
	<Variables>
		<Variable VarValue="[DNM]">name</Variable>
		<Variable VarValue="[DNM]">age</Variable>
	</Variables>
</MarkScheme>