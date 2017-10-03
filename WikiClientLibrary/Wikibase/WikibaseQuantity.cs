﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WikiClientLibrary.Wikibase
{
    public struct WikibaseQuantity
    {

        public WikibaseQuantity(double amount, WikibaseUri unit) : this(amount, amount, amount, unit)
        {
            
        }

        public WikibaseQuantity(double amount, double error, WikibaseUri unit) : this(amount, amount - error, amount + error, unit)
        {

        }

        public WikibaseQuantity(double amount, double lowerBound, double upperBound, WikibaseUri unit)
        {
            if (amount < lowerBound || amount > upperBound) throw new ArgumentException("amount should be between lowerBound and upperBound.");
            Amount = amount;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        // TODO Use more accurate decimal types.
        public double Amount { get; }

        public double LowerBound { get; }
        
        public double UpperBound { get; }

        public WikibaseUri Unit { get; }

        public bool HasError => Amount != LowerBound || Amount != UpperBound;

        /// <inheritdoc />
        public override string ToString()
        {
            var s = Amount.ToString();
            if (HasError)
            {
                var upper = UpperBound - Amount;
                var lower = Amount - LowerBound;
                if (upper - lower < 1e-14 || (upper - lower) / Amount < 1e-14)
                    s += "±" + upper;
                else
                    s += "+" + upper + "/-" + lower;
            }
            if (Unit != null) s += "(" + Unit + ")";
            return s;
        }
    }
}
