using BusinessObjects;
using Services.Interface;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for TagDetails.xaml
    /// </summary>
    public partial class TagDetails : Window
    {
        public TagDetails()
        {
            InitializeComponent();
        }

        public ITagSvc _tagSvc { get; set; }
        public bool InsertOrUpdate { get; set; }
        public Tag TagDetail { get; set; }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtTagId.IsEnabled = !InsertOrUpdate;
            if (InsertOrUpdate)
            {
                txtTagId.Text = TagDetail.TagId.ToString();
                txtTagName.Text = TagDetail.TagName;
                txtNote.Text = TagDetail.Note;
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tag tag = new Tag
                {
                    TagId = int.Parse(txtTagId.Text),
                    TagName = txtTagName.Text,
                    Note = txtNote.Text
                };

                if (InsertOrUpdate)
                {
                    _tagSvc.UpdateTag(tag);
                    MessageBox.Show("Tag " + tag.TagName + " updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _tagSvc.AddTag(tag);
                    MessageBox.Show("Tag " + tag.TagName + " added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add New Tag" : "Update A Tag");

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
