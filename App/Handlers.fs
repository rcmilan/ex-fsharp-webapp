module Handlers

    open System
    open System.Collections.Generic
    open Microsoft.AspNetCore.Http
    open Giraffe

    open Models
    open Todos

    let sayHelloNameHandler (name:string) =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let msg = sprintf "Hello, %s" name
                return! json { Response = msg } next ctx
            }

    let viewTasksHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()
                let todos = store.GetAll()
                return! json todos next ctx
            }

    let viewTaskHandler (id:Guid) =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()
                let todo = store.Get(id)
                return! json todo next ctx
            }

    let createTaskHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! newTodo = ctx.BindJsonAsync<NewTodo>()
                let store = ctx.GetService<TodoStore>()
                let created = store.Create({ Id = Guid.NewGuid(); Description = newTodo.Description; Created = DateTime.UtcNow; IsCompleted = false })
                return! json created next ctx
            }

    let updateTaskHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! todo = ctx.BindJsonAsync<Todo>()
                let store = ctx.GetService<TodoStore>()
                let created = store.Update(todo)
                return! json created next ctx
            }

    let deleteTaskHandler (id:Guid) =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()
                let existing = store.Get(id)
                let deleted = store.Delete(KeyValuePair<TodoId, Todo>(id, existing))
                return! json deleted next ctx
            }