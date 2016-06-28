﻿using ControlID;
using ControlID.iDAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAcesso
{
    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_Areas_Portal_Set()
        {
            // -------------
            // A - B - C - D
            long areaFora = eqpt.LoadOrAdd<Areas>("API Fora");
            long areaA = eqpt.LoadOrAdd<Areas>("API A");
            long areaB = eqpt.LoadOrAdd<Areas>("API B");
            //long areaC = eqpt.LoadOrAdd<Areas>("API C");
            //long areaD = eqpt.LoadOrAdd<Areas>("API D");
            //Console.WriteLine("Áreas Fora: " + areaFora + " Áreas Dentro: " + areaA + " / " + areaB + " / " + areaC + " / " + areaD);
            Console.WriteLine("Áreas Fora: " + areaFora + " Áreas Dentro: " + areaA + " / " + areaB);

            Portals portal1 = eqpt.LoadOrSet(1, new Portals() { name = "Portal 1", area_from_id = areaFora, area_to_id = areaA });
            Portals portal2 = eqpt.LoadOrSet(2, new Portals() { name = "Portal 2", area_from_id = areaFora, area_to_id = areaB });
            //Portals portal3 = eqpt.LoadOrSet(3, new Portals() { name = "Portal 3", area_from_id = areaFora, area_to_id = areaC });
            //Portals portal4 = eqpt.LoadOrSet(4, new Portals() { name = "Portal 4", area_from_id = areaFora, area_to_id = areaD });
            Console.WriteLine("Portais OK");

            //eqpt.LoadOrAdd<Portal_Actions>(new Portal_Actions() { portal_id = portal1.id, action_id = 2 });
            
        }

        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_Actions_Load()
        {
            Actions action1 = eqpt.Load<Actions>(1);
            Actions action2 = eqpt.Load<Actions>(2);
            Actions action3 = eqpt.Load<Actions>(3);
            Actions action4 = eqpt.Load<Actions>(4);
            Console.WriteLine("Relê OK");
        }

        [TestMethod(), TestCategory("sdk BOX")]
        public void sdk_Actions_Portal_List()
        {
            var actions = eqpt.List<Portal_Actions>();
        }

        [TestMethod(), TestCategory("sdk BOX")]
        public void sdk_ListAll()
        {
            Console.WriteLine("\r\nAccess_Rules");
            foreach (var ar in eqpt.List<Access_Rules>())
                Console.WriteLine(ar.id + ": " + ar.name + " priority: " + ar.priority + " type: " + ar.type);

            Console.WriteLine("\r\nAreas");
            foreach (var ar in eqpt.List<Areas>())
                Console.WriteLine(ar.id + ": " + ar.name);

            Console.WriteLine("\r\nPortals");
            foreach (var ar in eqpt.List<Portals>())
                Console.WriteLine(ar.id + ": " + ar.name);

            Console.WriteLine("\r\nTime_Zones");
            foreach (var ar in eqpt.List<Time_Zones>())
                Console.WriteLine(ar.id + ": " + ar.name);

            Console.WriteLine("\r\nGroups");
            foreach (var ar in eqpt.List<Groups>())
                Console.WriteLine(ar.id + ": " + ar.name);

            Console.WriteLine("\r\nPortal_Access_Rules");
            foreach (var ar in eqpt.List<Portal_Access_Rules>())
                Console.WriteLine(ar.access_rule_id + ": " + ar.portal_id);

            Console.WriteLine("\r\nArea_Access_Rules");
            foreach (var ar in eqpt.List<Area_Access_Rules>())
                Console.WriteLine(ar.access_rule_id + ": " + ar.area_id);

            Console.WriteLine("\r\nAccess_Rule_Time_Zones");
            foreach (var ar in eqpt.List<Access_Rule_Time_Zones>())
                Console.WriteLine(ar.access_rule_id + ": " + ar.time_zone_id);

            Console.WriteLine("\r\nGroup_Access_Rules");
            foreach (var ar in eqpt.List<Group_Access_Rules>())
                Console.WriteLine(ar.access_rule_id + ": " + ar.group_id);

            Console.WriteLine("\r\nUser_Access_Rules");
            foreach (var ar in eqpt.List<User_Access_Rules>())
                Console.WriteLine(ar.access_rule_id + ": " + ar.user_id);

            Console.WriteLine("\r\nPortal_Actions");
            foreach (var ar in eqpt.List<Portal_Actions>())
                Console.WriteLine(ar.portal_id  + ": " + ar.action_id);

            //var portal = eqpt.List<Portal_Access_Rules>();
            //eqpt.TryDestroyAll<Portal_Access_Rules>();
            //eqpt.ClearAreasTimesRules();

            //eqpt.Add(new Access_Rules() { id = 1, name = "Teste", type = 1, priority = 0 });
            ////eqpt.Add(new Area_Access_Rules() { area_id = 4, access_rule_id = 1 });
            //eqpt.Add(new Access_Rule_Time_Zones() {  time_zone_id = 1, access_rule_id = 1 });
            //eqpt.Add(new Group_Access_Rules() { group_id = 1, access_rule_id = 1 });
            //eqpt.Add(new Portal_Access_Rules() {  portal_id = 1, access_rule_id = 1 });

        }

        [TestMethod(), TestCategory("sdk BOX")]
        public void sdk_RuleTest()
        {
            //eqpt.Add(new Access_Rules() { id = 2, name = "Teste", type = 1, priority = 0 });
            //eqpt.Add(new User_Access_Rules() { access_rule_id = 2, user_id = 1 });
            //eqpt.Add(new Access_Rule_Time_Zones() { access_rule_id = 2, time_zone_id = 1 });
            eqpt.Add(new Portal_Access_Rules() { access_rule_id = 2,  portal_id = 1 });
        }

        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_User_Rule()
        {
            long area = eqpt.LoadOrAdd<Areas>("API B");
            long grupo = eqpt.LoadOrAdd<Groups>("Engenharia");
            Access_Rules rule = eqpt.LoadOrSet(0, new Access_Rules() { name = "Minha Regra", type = 1, priority = 0 });
            eqpt.SetGroupAreaTime(rule.id, grupo, area, 1);
            eqpt.Set(new User_Groups() { user_id = 1, group_id = grupo });
        }

       

        [TestMethod, TestCategory("sdk BOX")]
        public void sdk_LeitoraRele()
        {
            // Obtem s scripts relacionado a leitoras
            Scripts script = eqpt.First<Scripts>(new Scripts() { script = "card%" });
            Console.WriteLine("Script: " + script.script);

            // Obtem as leitoras disponíveis
            Script_Parameters[] sParameters = eqpt.List<Script_Parameters>(new Script_Parameters() { script_id = script.id });
            foreach (var prm in sParameters)
            {
                var rele_where = new Portal_Script_Parameters() { script_parameter_id = prm.id };
                Portal_Script_Parameters rele = eqpt.First<Portal_Script_Parameters>(rele_where);
                Console.WriteLine("Leitora: " + prm.name + " rele: " + rele.value);
            }

            long fora = eqpt.LoadOrAdd<Areas>("API Fora");
            long areaA = eqpt.LoadOrAdd<Areas>("API A");
            long areaB = eqpt.LoadOrAdd<Areas>("API B");
            long areaC = eqpt.LoadOrAdd<Areas>("API C");
            long areaD = eqpt.LoadOrAdd<Areas>("API D");

            // Altera a Leitora em relação ao Rele
            eqpt.SetWigandPortal(1, 1, fora, areaA);
            eqpt.SetWigandPortal(2, 2, fora, areaB);
            eqpt.SetWigandPortal(3, 3, fora, areaC);
            eqpt.SetWigandPortal(4, 4, fora, areaD);
        }
    }
}