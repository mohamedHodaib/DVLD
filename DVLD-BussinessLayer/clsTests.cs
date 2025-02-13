using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BussinessLayer
{
    public class clsTests
    {
        public int TestID { get; set; }
        public int TestAppointmentID {  get; set; }
        public bool TestResult {  get; set; }
        public string Notes {  get; set; }
        public int UserID {  get; set; }

        public clsTests()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.UserID = -1;
        }

        public clsTests(int testID, int testAppointmentID, bool testResult, string notes, int userID)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            UserID = userID;
        }

        private static void _AssignIsSystemErrorVariable()
        {
            clsGlobal.IsSystemError = DataAccessSettings.IsSystemError;
        }
        public static short GetPassedTests(int LDLAppID)
        {
            short passedTests = clsTestsData.GetPassedTests(LDLAppID);
            _AssignIsSystemErrorVariable();
            return passedTests;
        }
        public static int GetTrials(int LDLAppID, clsTestTypes.enTestType TestType)
        {
            int Trials = clsTestsData.GetTrials(LDLAppID,(int) TestType);
            _AssignIsSystemErrorVariable();
            return Trials;
        }

        public static clsTests GetTest(int TestAppointmentID)
        {
            int TestID = -1,UserID = -1;
            bool TestResult = false;
            string Notes = "";
            if (clsTestsData.FindTest(TestAppointmentID, ref TestID, ref TestResult, ref Notes, ref UserID)) 
                return new clsTests(TestID, TestAppointmentID, TestResult, Notes,UserID);
            else 
                return null;
                
        }


        private bool _AddNewTest()
        {
            this.TestID = clsTestsData.AddNewTestandGetID(this.TestAppointmentID,this.TestResult,
                this.Notes,this.UserID);
            return TestID != -1;
        }
        public bool Save()
        {
                    if (_AddNewTest())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
        }
    }
}
