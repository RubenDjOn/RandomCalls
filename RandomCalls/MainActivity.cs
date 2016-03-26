using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Provider;

namespace RandomCalls
{
    [Activity(Label = "RandomCalls", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private List<KeyValuePair<string,string>> contactList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.contactList = this.getContactList();

            Button randomCallButton = FindViewById<Button>(Resource.Id.callButton);
            randomCallButton.Enabled = this.contactList.Count > 0;
            randomCallButton.Click += (object sender, EventArgs e) =>
            {
                Random random = new Random();
                int randomNumber = random.Next(this.contactList.Count);
                var randomPhoneNumber = contactList.ElementAt(randomNumber).Value;

                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Llamar al número de teléfono: " + randomPhoneNumber + "?");
                callDialog.SetPositiveButton("Llamar", delegate {
                    Intent callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + randomPhoneNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancelar", delegate { });

                callDialog.Show();
            };
        }

        private List<KeyValuePair<string,string>> getContactList()
        {
            List<KeyValuePair<string, string>> contactList = new List<KeyValuePair<string, string>>();

            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;

            string[] projection = { ContactsContract.CommonDataKinds.Phone.Number,
       ContactsContract.Contacts.InterfaceConsts.DisplayName };

            var cursor = ManagedQuery(uri, projection, null, null, null);

            if (cursor.MoveToFirst())
            {
                do
                {
                    string name = cursor.GetString(
                            cursor.GetColumnIndex(projection[1]));
                    string phone = cursor.GetString(
                            cursor.GetColumnIndex(projection[0]));
                    KeyValuePair<string,string> contact = new KeyValuePair<string, string>(name, phone);
                    contactList.Add(contact);
                } while (cursor.MoveToNext());
            }

            return contactList;
        }
    }
}

