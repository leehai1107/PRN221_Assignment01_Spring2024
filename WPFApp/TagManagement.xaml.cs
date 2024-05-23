using BusinessObjects;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for TagManagement.xaml
    /// </summary>
    public partial class TagManagement : Window
    {
        private ITagSvc _tagSvc;

        public TagManagement()
        {
            _tagSvc = new TagSvc();
            InitializeComponent();
            LoadTags();
        }

        private void LoadTags()
        {
            List<Tag> tags = _tagSvc.GetTags();
            if (tags.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
            lvTags.ItemsSource = tags;
        }

        private void lvTags_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as Tag;
            if (item != null)
            {
                txtId.Text = item.TagId.ToString();
                txtTagName.Text = item.TagName;
                txtNote.Text = item.Note;
            }
        }

        private bool Validate(bool isIncludeId = true)
        {
            if (isIncludeId && txtId.Text.Trim().Length <= 0) return false;
            if (txtNote.Text.Trim().Length <= 0) { return false; }
            if (txtTagName.Text.Trim().Length <= 0) return false;

            return true;
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var data = txtId.Text.Trim();
            if (data.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this tag?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _tagSvc.DeleteTag(Convert.ToInt32((string)data));
                    LoadTags();
                }
            }
            else
            {
                MessageBox.Show("Please input tag data!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private Tag GetTagDetails()
        {
            Tag tagDetail = null;
            try
            {
                tagDetail = new Tag
                {
                    TagId = Convert.ToInt32(txtId.Text),
                    TagName = txtTagName.Text,
                    Note = txtNote.Text
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Tag");

            }
            return tagDetail;
        }

        private void btnNew_CLick(object sender, RoutedEventArgs e)
        {
            TagDetails tagDetails = new TagDetails
            {
                Title = "Add New Tag",
                InsertOrUpdate = false,
                _tagSvc = _tagSvc,
            };

            tagDetails.Show();
            tagDetails.Closed += (s, args) => LoadTags();

        }

        private void lvTags_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TagDetails tagDetails = new TagDetails
            {
                Title = "Update Tag",
                InsertOrUpdate = true,
                _tagSvc = _tagSvc,
                TagDetail = GetTagDetails(),
            };

            tagDetails.Show();
            tagDetails.Closed += (s, args) => LoadTags();

        }

        private void lvTags_Loaded(object sender, RoutedEventArgs e)
        {
            btnDelete.IsEnabled = false;
            LoadTags();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadTags();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Tag> tags = _tagSvc.SearchTagsByName(txtSearch.Text);
            if (tags.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
            lvTags.ItemsSource = tags;
        }
    }
}
