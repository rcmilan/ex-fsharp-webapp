module Routes
    open Microsoft.AspNetCore.Hosting
    open Giraffe

    open Models
    open Views

    let apiTodoRoutes : HttpHandler =
        subRoute "/todo"
            (choose [
                GET >=> choose [
                    routef "/%O" Handlers.viewTaskHandler
                    route "" >=> Handlers.viewTasksHandler
                ]
                PUT >=> route "" >=> Handlers.updateTaskHandler
                POST >=> route "" >=> Handlers.createTaskHandler
                DELETE >=> routef "/%O" Handlers.deleteTaskHandler
            ])

    let webApp : HttpHandler =
        choose [
            GET >=> route "/" >=> htmlView todoView
            subRoute "/api"
                (choose [
                    apiTodoRoutes
                    GET >=> route "" >=> json { Response = "ToDo List API" }
                    GET >=> routef "/%s" Handlers.sayHelloNameHandler
                ])
            setStatusCode 404 >=> text "Not Found"
        ]