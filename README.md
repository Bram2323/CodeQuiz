# Code Quiz
An application to test your code knowledge!

## Json format

### Question:
Every question can have:
- "title" - String
- "segments" - Segment Array

Every question must have:
- "answer" - Answer
---
### Segment:
Every segment can have:
- "text" - String

Every segment must have:
- "type" - Segment Type

#### Segment Types
- "text" - A rich text segment
- "code" - A code block with syntax highlighting
- "line" - A thin line (Ignores the "text" field)
---
### Answer
Every answer can have:
- "caseSensitive" - bool (false when not specified)
Every answer must have:
- "type" - Answer Type
- "answers" - Answer Option Array

#### Answer Types
- "list" - The user needs to guess all the answers, immediately reveals correctly guessed answers
- "multi" - The user needs to select all the correct option
- "single" - The user needs to select the correct option (If multiple options are correct it converts to a "multi" type)
- "line" - The user needs to guess all the answers, doesn't immediately reveal correctly guessed answers
---
### Answer Option
Every answer option can have:
- "correct" - bool (false if not specified)

Every answer option must have:
- "text" - String
