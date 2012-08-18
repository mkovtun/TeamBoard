namespace TeamBoard.Commands

open System

type ICommand =
    interface
    end

type ICommandSender =
    abstract member Send : ICommand -> unit