open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.ViewEngine

open Models
open Todos


let indexView =
    html [] [
        head [] [
            title [] [ str "Giraffe Example" ]
        ]
        body [] [
            h1 [] [ str "I |> F#" ]
            p [ _class "some-css-class"; _id "someId" ] [
                str "Hello World"
            ]
        ]
    ]

let sayHelloNameHandler (name:string) =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let msg = sprintf "Hello, %s" name
            return! json { Response = msg } next ctx
        }

let apiTodoRoutes : HttpHandler =
    subRoute "/todo"
        (choose [
            GET >=> choose [
                routef "/%O" Handlers.viewTaskHandler
                route "" >=> Handlers.viewTasksHandler
            ]
            POST >=> route "" >=> Handlers.updateTaskHandler
            PUT >=> route "" >=> Handlers.createTaskHandler
            DELETE >=> routef "/%O" Handlers.deleteTaskHandler
        ])

let webApp =
    choose [
        GET >=> route "/" >=> htmlView indexView
        subRoute "/api"
            (choose [
                apiTodoRoutes
                GET >=> route "" >=> json { Response = "ToDo List API" }
                GET >=> routef "/%s" Handlers.sayHelloNameHandler
            ])
        setStatusCode 404 >=> text "Not Found"
    ]
    
let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe()
            .AddSingleton<TodoStore>(TodoStore()) |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHost ->
            webHost
                .Configure(configureApp)
                .ConfigureServices(configureServices)
                |> ignore)
        .Build()
        .Run()
    0