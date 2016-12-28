// -----------------------------------------------------------------------
//  <copyright file="MockData.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Licensed under the MIT license. See LICENSE file in the project root for full license information.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;

    using Microsoft.Xbox.Services.Leaderboard;

    public static class MockData
    {
        private static readonly Random rng = new Random(24021524);

        public static LeaderboardResult CreateLeaderboardResult(string displayName, int rowCount, int columnCount, uint skipToRank, uint maxItems)
        {
            List<LeaderboardColumn> columns = Enumerable.Range(0, columnCount).Select(i => NextLeaderboardColumn()).ToList();
            List<LeaderboardRow> rows = Enumerable.Range(1, rowCount).Select(i => NextLeaderboardRow(columns, i)).ToList();

            LeaderboardResult result = new MockLeaderboardResult(
                displayName == null ? "Mock Leaderboard " + rng.Next() : displayName + " Leaderboard",
                columns,
                rows,
                skipToRank,
                maxItems);

            return result;
        }

        public static LeaderboardRow NextLeaderboardRow(IList<LeaderboardColumn> columns, int rank)
        {
            return new LeaderboardRow
            {
                Gamertag = NextGamertag(),
                Percentile = 0.99,
                Rank = (uint)rank,
                Values = columns.Select(c =>
                {
                    if (c.StatisticType == typeof(int))
                    {
                        return rng.Next(0, 1000).ToString();
                    }

                    if (c.StatisticType == typeof(double))
                    {
                        return rng.NextDouble().ToString();
                    }

                    return Guid.NewGuid().ToString();
                }).ToList()
            };
        }

        public static LeaderboardColumn NextLeaderboardColumn()
        {
            int id = rng.Next();
            Type statType;
            switch (0)
            {
                case 0:
                    statType = typeof(int);
                    break;
                case 1:
                    statType = typeof(double);
                    break;
                default:
                    statType = typeof(string);
                    break;
            }

            return new LeaderboardColumn
            {
                DisplayName = "Column " + id,
                StatisticName = "Stat" + id,
                StatisticType = statType,
            };
        }

        public static string NextGamertag()
        {
            return "User_" + rng.Next();
        }
    }

    public class MockLeaderboardResult : LeaderboardResult
    {
        private IList<LeaderboardRow> allRows;
        private uint offset;

        public MockLeaderboardResult(string displayName, IList<LeaderboardColumn> columns, IList<LeaderboardRow> allRows, uint skipToRank, uint maxItems)
        {
            this.DisplayName = displayName;

            this.Columns = columns;
            this.allRows = allRows;
            this.Rows = this.allRows.Skip((int)skipToRank).Take((int)maxItems).ToList();
            this.offset = skipToRank;

            this.TotalRowCount = (uint)this.allRows.Count;
            this.HasNext = skipToRank + maxItems < this.allRows.Count;
        }

        public override Task<LeaderboardResult> GetNextAsync(uint maxItems)
        {
            if (!this.HasNext)
            {
                return null;
            }

            LeaderboardResult result = new MockLeaderboardResult(this.DisplayName, this.Columns, this.allRows, this.offset + (uint)this.Rows.Count, maxItems);
            return Task.FromResult(result);
        }
    }
}