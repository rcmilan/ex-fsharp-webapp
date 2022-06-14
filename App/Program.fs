open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.ViewEngine

type PingModel = {
    Response: string
}

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

let webApp =
    choose [
        GET >=> choose [
            route "/" >=> htmlView indexView
            subRoute "/api"
                (choose [
                        route "" >=> json { Response = "Hello world!!" }
                        routef "/%s" sayHelloNameHandler
                ])
        ]
        setStatusCode 404 >=> text "Not Found"
    ]
    
let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

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