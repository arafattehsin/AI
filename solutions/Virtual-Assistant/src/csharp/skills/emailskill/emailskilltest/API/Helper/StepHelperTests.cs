﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EmailSkill;
using EmailSkill.Dialogs.Shared;
using EmailSkill.Model;
using EmailSkill.Util;
using EmailSkillTest.API.Fakes;
using Microsoft.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmailSkillTest.API.Helper
{
    [TestClass]
    public class StepHelperTests : EmailSkillDialog
    {
        private const string DialogId = "test";
        private MockDialogStateAccessor mockDialogStateAccessor;
        private MockEmailStateAccessor mockEmailStateAccessor;

        public StepHelperTests()
            : base(DialogId)
        {
            Services = new MockSkillConfiguration();

            this.mockEmailStateAccessor = new MockEmailStateAccessor();
            EmailStateAccessor = mockEmailStateAccessor.GetMock().Object;

            this.mockDialogStateAccessor = new MockDialogStateAccessor();
            DialogStateAccessor = mockDialogStateAccessor.GetMock().Object;

            ServiceManager = new MockServiceManager();
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
        }

        [TestMethod]
        public async Task GetNameListStringTest_OneOption()
        {
            // Mock data
            mockEmailStateAccessor.MockEmailSkillState = new EmailSkillState
            {
                FindContactInfor = new EmailSkillState.FindContactInformation()
            };

            mockEmailStateAccessor.MockEmailSkillState.FindContactInfor.Contacts = GetRecipients(1);

            mockEmailStateAccessor.SetMockBehavior();
            EmailStateAccessor = mockEmailStateAccessor.GetMock().Object;

            var nameList = await GetNameListStringAsync(null);

            Assert.AreEqual(nameList, "test0: test0@test.com");
        }

        [TestMethod]
        public async Task GetNameListStringTest_TwoOptions()
        {
            // Mock data
            mockEmailStateAccessor.MockEmailSkillState = new EmailSkillState
            {
                FindContactInfor = new EmailSkillState.FindContactInformation()
            };

            mockEmailStateAccessor.MockEmailSkillState.FindContactInfor.Contacts = GetRecipients(2);

            mockEmailStateAccessor.SetMockBehavior();
            EmailStateAccessor = mockEmailStateAccessor.GetMock().Object;

            var nameList = await GetNameListStringAsync(null);

            Assert.AreEqual(nameList, "test0: test0@test.com and test1: test1@test.com");
        }

        [TestMethod]
        public async Task GetNameListStringTest_ThreeOptions()
        {
            // Mock data
            mockEmailStateAccessor.MockEmailSkillState = new EmailSkillState
            {
                FindContactInfor = new EmailSkillState.FindContactInformation()
            };

            mockEmailStateAccessor.MockEmailSkillState.FindContactInfor.Contacts = GetRecipients(3);

            mockEmailStateAccessor.SetMockBehavior();
            EmailStateAccessor = mockEmailStateAccessor.GetMock().Object;

            var nameList = await GetNameListStringAsync(null);

            Assert.AreEqual(nameList, "test0: test0@test.com, test1: test1@test.com and test2: test2@test.com");
        }

        [TestMethod]
        public void FormatRecipientListTest()
        {
            var personData = GetPersonLists(0, 5);
            var contactData = GetPersonLists(1, 6);
            personData.AddRange(contactData);

            List<PersonModel> originPersonList = personData;
            List<PersonModel> originUserList = GetPersonLists(2, 7);

            (var personList, var userList) = DisplayHelper.FormatRecipientList(originPersonList, originUserList);

            Assert.AreEqual(personList.Count, 6);
            Assert.AreEqual(userList.Count, 1);
        }

        private List<Recipient> GetRecipients(int count)
        {
            List<Recipient> result = new List<Recipient>();

            for (int i = 0; i < count; i++)
            {
                var recipient = new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Name = "test" + i.ToString(),
                        Address = "test" + i.ToString() + "@test.com"
                    }
                };

                result.Add(recipient);
            }

            return result;
        }

        private List<PersonModel> GetPersonLists(int start, int end)
        {
            List<PersonModel> result = new List<PersonModel>();

            for (int i = start; i < end; i++)
            {
                var emailList = new List<string>();
                emailList.Add("test" + i.ToString() + "@test.com");

                var person = new PersonModel
                {
                    DisplayName = "test" + i.ToString(),
                    Emails = emailList
                };

                result.Add(person);
            }

            return result;
        }
    }
}