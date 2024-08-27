# Code Quiz
An application to create simple code questions!
To quiz yourself you can select a directory and every .question file in that directory and sub-directories will be loaded into a quiz
You can use the built in editor or any text editor to create questions

## Question Format
Code Quiz uses a custom .question format which is mostly plain text with some styling:
### Styling:
- `#` - Creates a heading with a line underneath
- `---` - Creates a line across the screen
- ```` ``` ```` - Starts or ends a code block with syntax highlighting (Currently only for java)

`=== [type]` - Starts a answer block with a type
| Type | Description         |
|---|---------------------|
|`List`|Guess all the answers one by one (Fails if you guess an answer marked as incorrect)|
|`Line`|Input all answers in a text edit (Ignores if answers are marked as incorrect/correct)|
|`Multi`|Select all the correct answers|
|`Single`|Select any of the correct answers|

In the answer block you can define answers like this:
- `+` - A correct answer that is not case sensitive
- `++` - A correct answer that is case sensitive
- `-` - An incorrect answer that is not case sensitive
- `--` - An incorrect answer that is case sensitive

`===` - Ends an answer block

### Example:
````
# How many times will the for loop run?
```
public class Loops {
    public static void main(String[] args){
	    int j = 5;
	    for (int i = 2; i <= 6; i++){
	        j += i;
	    }
	    System.out.println(j);
    }
}
```
In the file "Loops.java"

=== Single
- 7
- 6
+ 5
- 4
- Compile Error
===
````
