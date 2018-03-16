using System.Management.Automation;
using Product.Framework.Forms;
using Product.Framework.Forms.NewProjectWizardForms;
using Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms;
using Product.Framework.Forms.NewProjectWizardForms.IntegrationForms;
using Product.Framework.Forms.ProfileForms;
using Product.Framework.Forms.ProfileForms.WizardForms;
using Product.Framework.Forms.PublicFolderMigrationForms;

namespace Product.Framework.Steps
{
	public class UserSteps : BaseEntity
	{
		public MainForm AtMainForm()
		{
			return new MainForm();
		}

		public RegistrationForm AtRegistrationForm()
		{
			return new RegistrationForm();
		}

		public ChooseYourProjectTypeForm AtChooseYourProjectTypeForm()
		{
			return new ChooseYourProjectTypeForm();
		}

		public PublicFolderMigrationViewForm AtPublicFolderMigrationViewForm()
		{
			return new PublicFolderMigrationViewForm();
		}

		public GroupsMigrationForm AtGroupsMigrationForm()
		{
			return new GroupsMigrationForm();
		}

		public SyncScheduleForm AtSyncScheduleForm()
		{
			return new SyncScheduleForm();
		}

		public PublicFolderMigrtationListForm AtPublicFolderMigrtationListForm()
		{
			return new PublicFolderMigrtationListForm();
		}

		public EnablePublicFoldersForm AtEnablePublicFoldersForm()
		{
			return  new EnablePublicFoldersForm();
		}

	    public PublicFolderListForm AtPublicFolderListForm()
	    {
            return new PublicFolderListForm();
	    }

	    public PublicFolderTenantPareForm AtTenantPareForm()
		{
			return new PublicFolderTenantPareForm();
		}

		public SetProjectNameForm AtSetProjectNameForm()
		{
			return new SetProjectNameForm();
		}

		public PublicFolderSourceFilePathForm AtPublicFolderSourceFilePathForm()
		{
			return new PublicFolderSourceFilePathForm();
		}

		public PublicFolderTargetFilePathForm AtPublicFolderTargetFilePathForm()
		{
			return new PublicFolderTargetFilePathForm();
		}

		public PublicFolderSyncLevelForm AtPublicFolderSyncLevelForm()
		{
			return new PublicFolderSyncLevelForm();
		}

		public HowToMatchGroupsForm AtHowToMatchGroupsForm()
		{
			return new HowToMatchGroupsForm();
		}

		public UserMigrationExpirienceForm AtUserMigrationExpirienceForm()
		{
			return new UserMigrationExpirienceForm();
		}

		public MigrateDistributionGroupsForm AtMigrateDistributionGroupsForm()
		{
			return new MigrateDistributionGroupsForm();
		}

		public PublicFolderCompleteForm AtPublicFolderCompleteForm()
		{
			return new PublicFolderCompleteForm();
		}

		public PublicFolderConflictsForm AtPublicFolderConflictsForm()
		{
			return new PublicFolderConflictsForm();
		}

		public PublicFoldersScheduleForm AtPublicFolderScheduleForm()
		{
			return new PublicFoldersScheduleForm();
		}

		public SetProjectDescriptionForm AtSetProjectDescriptionForm()
		{
			return new SetProjectDescriptionForm();
		}

		public Office365LoginForm AtOffice365LoginForm()
		{
			return new Office365LoginForm();
		}

		public Office365AccountTypeForm AtOffice365AccountTypeForm()
		{
			return new Office365AccountTypeForm();
		}

		public DiscoveryIsCompleteForm AtDiscoveryIsCompleteForm()
		{
			return new DiscoveryIsCompleteForm();
		}

		public AddTenantsForm AtAddTenantsForm()
		{
			return new AddTenantsForm();
		}

		public UploadFilesForm AtUploadFilesForm()
		{
			return new UploadFilesForm();
		}

		public SyncAddressBooksForm AtSyncAddressBooksForm()
		{
			return new SyncAddressBooksForm();
		}

		

		public ShareCalendarForm AtShareCalendarForm()
		{
			return new ShareCalendarForm();
		}

		public EnterPasswordForm AtEnterPasswordForm()
		{
			return new EnterPasswordForm();
		}

		public DirectorySyncSettingsForm AtDirectorySyncSettingsForm()
		{
			return new DirectorySyncSettingsForm();
		}

	    public DownloadDirSyncForm AtDownloadDirSyncForm()
	    {
            return new DownloadDirSyncForm();
	    }

	    public DirectorySyncStatusForm AtDirectorySyncStatusForm()
		{
			return new DirectorySyncStatusForm();
		}

		public UploadedUsersForm AtUploadedUsersForm()
		{
			return new UploadedUsersForm();
		}

		public SelectTargetTenantForm AtSelectTargetTenantForm()
		{
			return new SelectTargetTenantForm();
		}

		public CreateDistributionGroupsForm AtCreateDistributionGroupsForm()
		{
			return new CreateDistributionGroupsForm();
		}

		public ReviewTenantPairsForm AtReviewTenantPairsForm()
		{
			return new ReviewTenantPairsForm();
		}

		public SelectSourceDomainForm AtSelectSourceDomainForm()
		{
			return new SelectSourceDomainForm();
		}

		public ReviewDomainsPairsForm AtReviewDomainsPairsForm()
		{
			return new ReviewDomainsPairsForm();
		}

		public WhichUsersShareCalendarForm AtWhichUsersShareCalendarForm()
		{
			return new WhichUsersShareCalendarForm();
		}

		public CalendarActiveDirectoryGroupForm AtCalendarActiveDirectoryGroupForm()
		{
			return new CalendarActiveDirectoryGroupForm();
		}

		public MigrationTypeForm AtMigrationTypeForm()
		{
			return new MigrationTypeForm();
		}

		public SelectMigrationGroupForm AtSelectMigrationGroupForm()
		{
			return new SelectMigrationGroupForm();
		}

		public ReviewGroupsForm AtReviewGroupsForm()
		{
			return new ReviewGroupsForm();
		}

		public HowToMatchUsersForm AtHowToMatchUsersForm()
		{
			return new HowToMatchUsersForm();
		}

		public DiscoveryIsInProgressForm AtDiscoveryIsInProgressForm()
		{
			return new DiscoveryIsInProgressForm();
		}

		public DiscoveryProgressForm AtDiscoveryProgressForm()
		{
			return new DiscoveryProgressForm();
		}

		public KeepUsersForm AtKeepUsersForm()
		{
			return new KeepUsersForm();
		}

		public CreateUsersForm AtCreateUsersForm()
		{
			return new CreateUsersForm();
		}

		public UploadDistributionListForm AtUploadDistributionListForm()
		{
			return new UploadDistributionListForm();
		}

		public ConfigureDirectorySyncForm AtConfigureDirectorySyncForm()
		{
			return new ConfigureDirectorySyncForm();
		}

		public EmailRewritingForm AtEmailRewritingForm()
		{
			return new EmailRewritingForm();
		}

		public ConfigureEmailRewrittingForm AtConfigureEmailRewrittingForm()
		{
			return new ConfigureEmailRewrittingForm();
		}

		public PublicFolderTenantPareForm AtPublicFolderTenantPareForm()
		{
			return new PublicFolderTenantPareForm();
		}

		public AnyForm AtAnyForm()
		{
			return new AnyForm();
		}

		public GoodToGoForm AtGoodToGoForm()
		{
			return new GoodToGoForm();
		}

		public MigrationWavesForm AtMigrationWavesForm()
		{
			return new MigrationWavesForm();
		}

		public SelectTargetDomainForm AtSelectTargetDomainForm()
		{
			return new SelectTargetDomainForm();
		}

		public DefineMigrationWavesForm AtDefineMigrationWavesForm()
		{
			return new DefineMigrationWavesForm();
		}

		public BeginDiscoveryForm AtBeginDiscoveryForm()
		{
			return new BeginDiscoveryForm();
		}

		public AlmostDoneForm AtAlmostDoneForm()
		{
			return new AlmostDoneForm();
		}

		public SelectSourceTenantForm AtSelectSourceTenantForm()
		{
			return new SelectSourceTenantForm();
		}

		public TenantRestructuringForm AtTenantRestructuringForm()
		{
			return new TenantRestructuringForm();
		}

		public ProjectDetailsForm AtProjectDetailsForm()
		{
			return new ProjectDetailsForm();
		}

		public CmtLoginForm AtCmtLoginForm()
		{
			return new CmtLoginForm();
		}

		public SubmittedProjectForm AtSubmittedProjectForm()
		{
			return new SubmittedProjectForm();
		}

		public ProjectOverviewForm AtProjectOverviewForm()
		{
			return new ProjectOverviewForm();
		}

		public UsersForm AtUsersForm()
		{
			return new UsersForm();
		}

		public TenantsConfigurationForm AtTenantsConfigurationForm()
		{
			return new TenantsConfigurationForm();
		}

	    public ProfilesOverviewForm AtProfilesOverviewForm()
	    {
            return new ProfilesOverviewForm();
	    }

	    public ProfileNameForm AtProfileNameForm()
	    {
            return new ProfileNameForm();
	    }

	    public ProfileOutlookConfigForm AtProfileOutlookConfigForm()
	    {
            return new ProfileOutlookConfigForm();
	    }

	    public ProfileMailboxUpdateForm AtProfileMailboxUpdateForm()
	    {
            return new ProfileMailboxUpdateForm();
	    }

	    public ProfileCreateUsersForm AtProfileCreateUsersForm()
	    {
            return new ProfileCreateUsersForm();
	    }

	    public ProfileCreateDistributionGroupsForm AtProfileCreateDistributionGroupsForm()
	    {
            return new ProfileCreateDistributionGroupsForm();
	    }

	    public ProfileSyncDistributionGroupsForm AtProfileSyncDistributionGroupsForm()
	    {
            return new ProfileSyncDistributionGroupsForm();
	    }

        public ProfileContentToMigrateForm AtProfileContentToMigrateForm => new ProfileContentToMigrateForm();
        public ProfileFilterMessagesForm AtProfileFilterMessagesForm => new ProfileFilterMessagesForm();
        public ProfileFilterCalendarForm AtProfileFilterCalendarForm => new ProfileFilterCalendarForm();
        public ProfileFilterTasksForm AtProfileFilterTasksForm => new ProfileFilterTasksForm();
        public ProfileFilterNoteForm AtProfileFilterNoteForm => new ProfileFilterNoteForm();
        public ProfileLargeItemsHandleForm AtProfileLargeItemsHandleForm => new ProfileLargeItemsHandleForm();
	}
}