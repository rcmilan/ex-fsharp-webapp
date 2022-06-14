module Todos

    open System.Collections.Concurrent

    open Models

    type TodoStore() =
        let data = ConcurrentDictionary<TodoId, Todo>()

        member _.Create todo = data.TryAdd(todo.Id, todo)
        member _.Update todo = data.TryUpdate(todo.Id, todo, data.[todo.Id])
        member _.Delete id = data.TryRemove id
        member _.Get id = data.[id]
        member _.GetAll () = data.ToArray()