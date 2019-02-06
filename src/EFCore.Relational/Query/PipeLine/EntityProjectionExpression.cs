﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Relational.Query.PipeLine
{
    public class EntityProjectionExpression : Expression
    {
        private readonly IDictionary<IProperty, SqlExpression> _propertyExpressionCache = new Dictionary<IProperty, SqlExpression>();
        private readonly TableExpressionBase _innerTable;

        public EntityProjectionExpression(IEntityType entityType, TableExpressionBase innerTable)
        {
            EntityType = entityType;
            _innerTable = innerTable;
        }

        public IEntityType EntityType { get; }

        public SqlExpression GetProperty(IProperty property)
        {
            if (!_propertyExpressionCache.TryGetValue(property, out var expression))
            {
                expression = new SqlExpression(new ColumnExpression(property, _innerTable), property.FindRelationalMapping());
                _propertyExpressionCache[property] = expression;
            }

            return expression;
        }
    }
}