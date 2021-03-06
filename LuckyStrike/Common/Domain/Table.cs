﻿using System.Collections.Generic;

using Common.Abstract;

namespace Common.Domain
{
    public class Table
    {
        public int Id { get; private set; }
        public int Size { get; private set; }
        public int Dealer { get; private set; }
        public List<AbstractSeat> Seats { get; private set; }
        public List<Card> Board { get; private set; }

        public int ActivePlayersCount { get; set; }

        public Street Street
        {
            get
            {
                switch (this.Board.Count)
                {
                    case 0:
                        return Street.PREFLOP;
                    case 3:
                        return Street.FLOP;
                    case 4:
                        return Street.TURN;
                    default: //case 5
                        return Street.RIVER;
                }
            }
        }

        public Table(int id, int size, AbstractStrategy strategy, AbstractOutput output)
        {
            this.Id = id;
            this.Size = size;
            this.Seats = new List<AbstractSeat>();
            this.Board = new List<Card>();
            this.Board.Clear();

            AbstractSeat prevSeat = new EmptySeat(this, null, null);
            for (int i = 1; i < this.Size; i++)
            {
                this.Seats.Add(prevSeat);
                prevSeat = new EmptySeat(this, null, prevSeat);
            }
            this.Seats.Add(prevSeat);

            this.Seats[0].Right = prevSeat;

            while (prevSeat.Right.Left == null)
            {
                prevSeat.Right.Left = prevSeat;
                prevSeat = prevSeat.Right;
            }

            this.SitIn(0, new ArtificialPlayer(strategy, output));
        }

        public void NewHand(int dealer)
        {
            this.Dealer = dealer;
            this.Board.Clear();
            this.ActivePlayersCount = 0;

            foreach (var abstractSeat in this.Seats)
            {
                if (abstractSeat is NonEmptySeat)
                {
                    (abstractSeat as NonEmptySeat).ResetActivity();
                    this.ActivePlayersCount++;
                }
            }
        }

        public void SitIn(int id, AbstractPlayer player)
        {
            var seat = new NonEmptySeat(this, this.Seats[id].Left, this.Seats[id].Right, player);
            seat.Left.Right = seat;
            seat.Right.Left = seat;

            player.Seats.Add(seat);

            this.Seats[id] = seat;
        }

        public void SitOut(int id)
        {
            this.Seats[id] = new EmptySeat(this, this.Seats[id].Left, this.Seats[id].Right);
        }

        public void NextStreet(Card card)
        {
            this.Board.Add(card);
        }

        public void NextStreet(List<Card> cards)
        {
            this.Board = cards;
        }
    }
}
