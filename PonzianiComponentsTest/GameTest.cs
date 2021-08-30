﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PonzianiComponents.Chesslib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PonzianiComponentsTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            Game game = new Game();
            Assert.IsNull(game.White);
            Assert.IsNull(game.Black);
            Assert.IsNull(game.Event);
            Assert.IsNull(game.Site);
            Assert.IsNull(game.Date);
            Assert.IsNull(game.Round);
            Assert.IsNull(game.Eco);
            Assert.AreEqual(Fen.INITIAL_POSITION, game.StartPosition);
            Assert.AreEqual(Fen.INITIAL_POSITION, game.Position.FEN);
            Assert.AreEqual(Result.OPEN, game.Result);
            Assert.AreEqual(ResultDetail.UNKNOWN, game.ResultDetail);
            Assert.AreEqual(Side.WHITE, game.SideToMove);
            Assert.AreEqual(0, game.Moves.Count);
            Assert.AreEqual(0, game.Tags.Count);
            string fen = "rnbqr1k1/pp3pbp/3p1np1/2pP4/4P3/2N2N2/PPQ1BPPP/R1B2RK1 b - - 7 10";
            game = new Game(fen);
            Assert.IsNull(game.White);
            Assert.IsNull(game.Black);
            Assert.IsNull(game.Event);
            Assert.IsNull(game.Site);
            Assert.IsNull(game.Date);
            Assert.IsNull(game.Round);
            Assert.IsNull(game.Eco);
            Assert.AreEqual(fen, game.StartPosition);
            Assert.AreEqual(fen, game.Position.FEN);
            Assert.AreEqual(Result.OPEN, game.Result);
            Assert.AreEqual(ResultDetail.UNKNOWN, game.ResultDetail);
            Assert.AreEqual(Side.BLACK, game.SideToMove);
            Assert.AreEqual(0, game.Moves.Count);
            Assert.AreEqual(0, game.Tags.Count);
        }

        [TestMethod]
        public void PlayLegalTrap()
        {
            Game game = new Game();
            string[] moves = "e2e4 e7e5 g1f3 b8c6 f1c4 d7d6 b1c3 c8g4 h2h3 g4h5 f3e5 h5d1 c4f7 e8e7 c3d5".Split(' ');
            foreach (string m in moves) game.Add(new ExtendedMove(m));
            Assert.AreEqual(Result.WHITE_WINS, game.Result);
            Assert.AreEqual(ResultDetail.MATE, game.ResultDetail);
            Assert.AreEqual(moves.Length, game.Moves.Count);
            Assert.AreEqual(Fen.INITIAL_POSITION, game.StartPosition);
            Assert.AreEqual("r2q1bnr/ppp1kBpp/2np4/3NN3/4P3/7P/PPPP1PP1/R1BbK2R b KQ - 2 8", game.Position.FEN);
        }

        [TestMethod]
        public void TestToPGN()
        {
            List<Game> games = PGN.Parse(Data.PGN_TWIC);
            List<Game> copiedGames = new List<Game>();
            foreach (var game in games)
            {
                Game ngame = new Game(game.StartPosition);
                ngame.White = game.White;
                ngame.Black = game.Black;
                ngame.Event = game.Event;
                ngame.Site = game.Site;
                foreach (var key in game.Tags.Keys) ngame.Tags.Add(key, game.Tags[key]);
                ngame.Date = game.Date;
                ngame.Result = game.Result;
                ngame.ResultDetail = game.ResultDetail;
                ngame.Round = game.Round;
                foreach (var m in game.Moves) ngame.Add(m);
                copiedGames.Add(ngame);
            }
            for (int i = 0; i < copiedGames.Count; ++i)
            {
                Game ngame = PGN.Parse(copiedGames[i].ToPGN()).First();
                Game game = games[i];
                Assert.AreEqual(game.White, ngame.White);
                Assert.AreEqual(game.Black, ngame.Black);
                Assert.AreEqual(game.Event, ngame.Event);
                Assert.AreEqual(game.Site, ngame.Site);
                Assert.AreEqual(game.Date, ngame.Date);
                Assert.AreEqual(game.Result, ngame.Result);
                Assert.AreEqual(game.ResultDetail, ngame.ResultDetail);
                Assert.AreEqual(game.Round, ngame.Round);
                Assert.IsTrue(game.Tags.Count <= ngame.Tags.Count);
                foreach (var key in game.Tags.Keys) Assert.AreEqual(game.Tags[key], ngame.Tags[key]);
                Assert.AreEqual(game.Moves.Count, ngame.Moves.Count);
                for (int j = 0; j < game.Moves.Count; ++j) Assert.AreEqual(game.Moves[j].ToUCIString(), ngame.Moves[j].ToUCIString());
            }
        }

    }
}