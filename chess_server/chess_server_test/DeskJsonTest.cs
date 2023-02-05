using System;
using System.Collections.Generic;
using System.Threading;
using chess_shared.Net;
using Chess.Model;
using Chess.Server;
using Net;
using NUnit.Framework;

namespace chess_server_test;

public class DeskJsonTest
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void JsonTest()
    {
        var desk = new Desk();
        desk.CreateMap();
        var json = ChessJsonSerializer.SerializeObj(desk);
        Console.WriteLine(json);
        var deskObj = new DeskObj(desk);
        ChessJsonSerializer.Populate(json, deskObj);
        Console.WriteLine(deskObj);
    }
    
}