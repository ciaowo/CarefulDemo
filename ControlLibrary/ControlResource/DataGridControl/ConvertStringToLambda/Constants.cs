﻿using System.Linq.Expressions;

namespace Careful.Controls.DataGridControl.ConvertStringToLambda
{
    internal static class Constants
    {
        public static readonly Expression TrueLiteral = Expression.Constant(true);
        public static readonly Expression FalseLiteral = Expression.Constant(false);
        public static readonly Expression NullLiteral = Expression.Constant(null);
    }
}
