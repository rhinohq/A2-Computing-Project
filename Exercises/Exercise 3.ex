[ExID]
3

[ExTitle]
Introducing If Statements

[ExDescription]
In many programming languages, including Lua, we use if statements to tell the computer what we want to happen next, depending on a certain condition. In the editor, there is an example of an if statement. 

In the example, change the word "condition" to 'num < 15'.

[ExCodeTemplate]
num = 5

if condition then
	print("num is less than 15.")
end

[ExAppendCode]

[ExMarkScheme]
<?xml version="1.0" encoding="utf-8"?>
<MarkScheme>
	<Output>num is less than 15.</Output>
	<ControlStructures StructType="IF">num &lt; 15</ControlStructures>
</MarkScheme>