[ExID]
3

[ExTitle]
Arithmetic

[ExDescription]
In programming, the abilty to do math is very important. Lua provides a few arithmetic operators in order for us to do this. The addition operator '+', the subtraction operator '-', the multiplication operator '*', the division operator '/' and the exponential operator '^'.</p>

[ExCodeTemplate]
num1 = -- 5 plus 5\rnum2 = -- 3 minus 2\rnum3 = -- 7 times 7 times 7\rnum4 = -- 9 divided by 3\rnum5 = -- 2 to the power of 8

[ExAppendCode]
print(string.format("num1 = {0}", num1))
print(string.format("num2 = {0}", num2))
print(string.format("num3 = {0}", num3))
print(string.format("num4 = {0}", num4))
print(string.format("num5 = {0}", num5))

[ExMarkScheme]
<?xml version="1.0" encoding="utf-8"?>
<MarkScheme>
	<Variables>
		<Variable VarValue="10">num1</Variable>
		<Variable VarValue="1">num2</Variable>
		<Variable VarValue="343">num3</Variable>
		<Variable VarValue="3">num4</Variable>
		<Variable VarValue="256">num5</Variable>
	</Variables>
</MarkScheme>