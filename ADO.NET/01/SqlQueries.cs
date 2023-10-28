using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01
    {
    public static class SqlQueries
        {

        public const string getValues = @"SELECT v.[Name], COUNT(*)[TotalMinions]
                                    FROM Villains v
                                    JOIN MinionsVillains mv ON mv.VillainId = v.Id
                                    GROUP BY v.[Name]
                                    HAVING COUNT(*) > 3
                                    ORDER BY [TotalMinions]";

        public const string getVillianById = @"select name from Villains where Id = @id";

        public const string getOrderedMynionsByVillianID = @"select ROW_NUMBER() over (order by m.name) as RowNum, m.name, m.Age
                                                             from MinionsVillains as mv 
                                                             join Minions as m on m.id = mv.MinionId
                                                             where mv.VillainId = @id
                                                             order by m.Name";


        }
    }
