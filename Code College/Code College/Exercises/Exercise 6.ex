[ExID]
6

[ExTitle]
Else and Elseif Statements

[ExDescription]
If statements can also have "elseif" and "else" statements. An elseif statement is used in order to not use a load of if statements consecutively. Else statements are used when the conditions of the if and elseif statements are not met, they are used as a sort of fallback operation. 

The exercise below is for an online shopping website. The company offers free shipping if you spend £10 or more and a 5% discount if you spend £20 or more.

[ExCodeTemplate]
discount = 0.05\rshipping = 2\r\rsubtotal = 23.50 -- You can change this to what you want.\r\rif condition then\r\tprint("You are eligible for free shipping.")\r\ttotal = subtotal\relseif condition then\r\tprint("You are eligible for free shipping and a 5% discount.")\r\ttotal = subtotal * discount\relse\r\tprint("Thank you for your order.")\r\ttotal = subtotal + shipping\rend\r\rprint(total)

[ExAppendCode]

[ExMarkScheme]
<?xml version="1.0" encoding="utf-8"?>
<MarkScheme>
	<ControlStructures>
		<ControlStructure StructType="IF">subtotal &#62; 11</ControlStructure>
		<ControlStructure StructType="ELSEIF">subtotal &#62; 21</ControlStructure>
		<ControlStructure StructType="ELSE"></ControlStructure>
	</ControlStructures>
</MarkScheme>