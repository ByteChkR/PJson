# PJson Examples
## Plain Json
Normal Json is valid input for the transpiler.

## String Expansion
Using Strings can be expanded with variables and functions by using the Interpolated String Symbol `$`

```json
//Input:
{
    "Name": "Tim",
    "Greeting": $"Hello {Name}"
}

//Output:
{
    "Name": "Tim",
    "Greeting": "Hello Tim"
}
```

## Property Hiding

The `~` operator can be used to hide variables from the output

```json

//Input:
{
    ~"Name": "Tim",
    "Greeting": $"Hello {Name}"
}

//Output:
{
    "Greeting": "Hello Tim"
}

```

## Branching

```json

//Input:
{
    ~"AddContactData": true,
    ~"Name": "Tim",
    ~"ContactData": "mymail@example.com"
    $"{Name}": {
        "Name": Name,
        if(AddContactData) 
            "Contact": ContactData
    }
}

//Output if AddContactData=true:
{
    "Tim": {
        "Name": "Tim",
        "Contact": "mymail@example.com"
    }
}

//Output if AddContactData=false:
{
    "Tim": {
        "Name": "Tim"
    }
}
```
## Loops

```json

//Input
{
    ~"FruitNames": ["Apple", "Banana", "Cherry"],
    ~"Prices": {
        "Apple": 10,
        "Banana": 20,
        "Cherry": 30,
    },
    "Fruits": foreach(fruit in FruitNames)
    [
        {
            "Name": fruit,
            "Price": Prices[fruit]
        }
    ]
}


//Output
{
    "Fruits":
    [
        {
            "Name": "Apple",
            "Price": 10
        },
        {
            "Name": "Banana",
            "Price": 20
        },
        {
            "Name": "Cherry",
            "Price": 30
        }
    ]
}

```

## File Inclusion

Simple File Inclusion
```json

//Input file a.pjson
{
    "Name": "Tim",
    "Age": 24
}

//Input
{
    "Person": &"a.pjson"
}

//Output
{
    "Person": {
        "Name": "Tim",
        "Age": 24
    }
}

```

Insert In Parent File inclusion
```json

//Input file a.pjson
{
    "Name": "Tim",
    "Age": 24
}

//Input
{
    "Person": {
        &&"a.pjson"
    }
}

//Output
{
    "Person": {
        "Name": "Tim",
        "Age": 24
    }
}
```

