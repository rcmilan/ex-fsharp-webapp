module Views
    
    open System
    open Giraffe.ViewEngine

    open Models

    let todoList = [
        { Id = Guid.NewGuid(); Description = "Hit the gym"; Created = DateTime.UtcNow; IsCompleted = false }
        { Id = Guid.NewGuid(); Description = "Pay bills"; Created = DateTime.UtcNow; IsCompleted = true }
        { Id = Guid.NewGuid(); Description = "Meet George"; Created = DateTime.UtcNow; IsCompleted = false }
        { Id = Guid.NewGuid(); Description = "Buy eggs"; Created = DateTime.UtcNow; IsCompleted = false }
        { Id = Guid.NewGuid(); Description = "Read a book"; Created = DateTime.UtcNow; IsCompleted = false }
        { Id = Guid.NewGuid(); Description = "Organize office"; Created = DateTime.UtcNow; IsCompleted = true }
    ]

    let createPage msg content =
        html [] [
            head [] [
                title [] [ Text msg ]
                link [ _rel "stylesheet"; _href "main.css" ]
            ]
            body [] content
        ]
    let showListItem (todo:Todo) =
        let style = if todo.IsCompleted then [ _class "checked" ] else []
        li style [ Text todo.Description ]

    let todoView =
        createPage "My ToDo App" [
            div [ _id "myDIV"; _class "header" ] [
                h2 [] [ Text "My ToDo List" ]
                input [ _type "text"; _id "myInput"; _placeholder "Title..." ]
                span [ _class "addBtn"; _onclick "newElement()" ] [ Text "Add" ]
            ]
            ul [ _id "myUL" ] [
                for todo in todoList do showListItem todo
            ]
            script [ _src "main.js"; _type "text/javascript" ] []
        ]