using System;
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
        private Guid driverId;

        public UserSteps(Guid driverId)
        {
            this.driverId = driverId;
        }

		public MainForm AtMainForm()
		{
			return new MainForm(driverId);
		}

		public RegistrationForm AtRegistrationForm()
		{
			return new RegistrationForm(driverId);
		}

		public ChooseYourProjectTypeForm AtChooseYourProjectTypeForm()
		{
			return new ChooseYourProjectTypeForm(driverId);
		}

		public PublicFolderMigrationViewForm AtPublicFolderMigrationViewForm()
		{
			return new PublicFolderMigrationViewForm(driverId);
		}

		public GroupsMigrationForm AtGroupsMigrationForm()
		{
			return new GroupsMigrationForm(driverId);
		}

		public SyncScheduleForm AtSyncScheduleForm()
		{
			return new SyncScheduleForm(driverId);
		}

		public PublicFolderMigrtationListForm AtPublicFolderMigrtationListForm()
		{
			return new PublicFolderMigrtationListForm(driverId);
		}

		public EnablePublicFoldersForm AtEnablePublicFoldersForm()
		{
			return  new EnablePublicFoldersForm(driverId);
		}

	    public PublicFolderListForm AtPublicFolderListForm()
	    {
            return new PublicFolderListForm(driverId);
	    }

	    public PublicFolderTenantPareForm AtTenantPareForm()
		{
			return new PublicFolderTenantPareForm(driverId);
		}

		public SetProjectNameForm AtSetProjectNameForm()
		{
			return new SetProjectNameForm(driverId);
		}

		public PublicFolderSourceFilePathForm AtPublicFolderSourceFilePathForm()
		{
			return new PublicFolderSourceFilePathForm(driverId);
		}

		public PublicFolderTargetFilePathForm AtPublicFolderTargetFilePathForm()
		{
			return new PublicFolderTargetFilePathForm(driverId);
		}

		public PublicFolderSyncLevelForm AtPublicFolderSyncLevelForm()
		{
			return new PublicFolderSyncLevelForm(driverId);
		}

		public HowToMatchGroupsForm AtHowToMatchGroupsForm()
		{
			return new HowToMatchGroupsForm(driverId);
		}

		public UserMigrationExpirienceForm AtUserMigrationExpirienceForm()
		{
			return new UserMigrationExpirienceForm(driverId);
		}

		public MigrateDistributionGroupsForm AtMigrateDistributionGroupsForm()
		{
			return new MigrateDistributionGroupsForm(driverId);
		}

		public PublicFolderCompleteForm AtPublicFolderCompleteForm()
		{
			return new PublicFolderCompleteForm(driverId);
		}

		public PublicFolderConflictsForm AtPublicFolderConflictsForm()
		{
			return new PublicFolderConflictsForm(driverId);
		}

		public PublicFoldersScheduleForm AtPublicFolderScheduleForm()
		{
			return new PublicFoldersScheduleForm(driverId);
		}

		public SetProjectDescriptionForm AtSetProjectDescriptionForm()
		{
			return new SetProjectDescriptionForm(driverId);
		}

		public Office365LoginForm AtOffice365LoginForm()
		{
			return new Office365LoginForm(driverId);
		}

		public Office365AccountTypeForm AtOffice365AccountTypeForm()
		{
			return new Office365AccountTypeForm(driverId);
		}

		public DiscoveryIsCompleteForm AtDiscoveryIsCompleteForm()
		{
			return new DiscoveryIsCompleteForm(driverId);
		}

		public AddTenantsForm AtAddTenantsForm()
		{
			return new AddTenantsForm(driverId);
		}

		public UploadFilesForm AtUploadFilesForm()
		{
			return new UploadFilesForm(driverId);
		}

		public SyncAddressBooksForm AtSyncAddressBooksForm()
		{
			return new SyncAddressBooksForm(driverId);
		}

		

		public ShareCalendarForm AtShareCalendarForm()
		{
			return new ShareCalendarForm(driverId);
		}

		public EnterPasswordForm AtEnterPasswordForm()
		{
			return new EnterPasswordForm(driverId);
		}

		public DirectorySyncSettingsForm AtDirectorySyncSettingsForm()
		{
			return new DirectorySyncSettingsForm(driverId);
		}

	    public DownloadDirSyncForm AtDownloadDirSyncForm()
	    {
            return new DownloadDirSyncForm(driverId);
	    }

	    public DirectorySyncStatusForm AtDirectorySyncStatusForm()
		{
			return new DirectorySyncStatusForm(driverId);
		}

		public UploadedUsersForm AtUploadedUsersForm()
		{
			return new UploadedUsersForm(driverId);
		}

		public SelectTargetTenantForm AtSelectTargetTenantForm()
		{
			return new SelectTargetTenantForm(driverId);
		}

		public CreateDistributionGroupsForm AtCreateDistributionGroupsForm()
		{
			return new CreateDistributionGroupsForm(driverId);
		}

		public ReviewTenantPairsForm AtReviewTenantPairsForm()
		{
			return new ReviewTenantPairsForm(driverId);
		}

		public SelectSourceDomainForm AtSelectSourceDomainForm()
		{
			return new SelectSourceDomainForm(driverId);
		}

		public ReviewDomainsPairsForm AtReviewDomainsPairsForm()
		{
			return new ReviewDomainsPairsForm(driverId);
		}

		public WhichUsersShareCalendarForm AtWhichUsersShareCalendarForm()
		{
			return new WhichUsersShareCalendarForm(driverId);
		}

		public CalendarActiveDirectoryGroupForm AtCalendarActiveDirectoryGroupForm()
		{
			return new CalendarActiveDirectoryGroupForm(driverId);
		}

		public MigrationTypeForm AtMigrationTypeForm()
		{
			return new MigrationTypeForm(driverId);
		}

		public SelectMigrationGroupForm AtSelectMigrationGroupForm()
		{
			return new SelectMigrationGroupForm(driverId);
		}

		public ReviewGroupsForm AtReviewGroupsForm()
		{
			return new ReviewGroupsForm(driverId);
		}

		public HowToMatchUsersForm AtHowToMatchUsersForm()
		{
			return new HowToMatchUsersForm(driverId);
		}

		public DiscoveryIsInProgressForm AtDiscoveryIsInProgressForm()
		{
			return new DiscoveryIsInProgressForm(driverId);
		}

		public DiscoveryProgressForm AtDiscoveryProgressForm()
		{
			return new DiscoveryProgressForm(driverId);
		}

		public KeepUsersForm AtKeepUsersForm()
		{
			return new KeepUsersForm(driverId);
		}

		public CreateUsersForm AtCreateUsersForm()
		{
			return new CreateUsersForm(driverId);
		}

		public UploadDistributionListForm AtUploadDistributionListForm()
		{
			return new UploadDistributionListForm(driverId);
		}

		public ConfigureDirectorySyncForm AtConfigureDirectorySyncForm()
		{
			return new ConfigureDirectorySyncForm(driverId);
		}

		public EmailRewritingForm AtEmailRewritingForm()
		{
			return new EmailRewritingForm(driverId);
		}

		public ConfigureEmailRewrittingForm AtConfigureEmailRewrittingForm()
		{
			return new ConfigureEmailRewrittingForm(driverId);
		}

		public PublicFolderTenantPareForm AtPublicFolderTenantPareForm()
		{
			return new PublicFolderTenantPareForm(driverId);
		}

		public AnyForm AtAnyForm()
		{
			return new AnyForm(driverId);
		}

		public GoodToGoForm AtGoodToGoForm()
		{
			return new GoodToGoForm(driverId);
		}

		public MigrationWavesForm AtMigrationWavesForm()
		{
			return new MigrationWavesForm(driverId);
		}

		public SelectTargetDomainForm AtSelectTargetDomainForm()
		{
			return new SelectTargetDomainForm(driverId);
		}

		public DefineMigrationWavesForm AtDefineMigrationWavesForm()
		{
			return new DefineMigrationWavesForm(driverId);
		}

		public BeginDiscoveryForm AtBeginDiscoveryForm()
		{
			return new BeginDiscoveryForm(driverId);
		}

		public AlmostDoneForm AtAlmostDoneForm()
		{
			return new AlmostDoneForm(driverId);
		}

		public SelectSourceTenantForm AtSelectSourceTenantForm()
		{
			return new SelectSourceTenantForm(driverId);
		}

		public TenantRestructuringForm AtTenantRestructuringForm()
		{
			return new TenantRestructuringForm(driverId);
		}

		public ProjectDetailsForm AtProjectDetailsForm()
		{
			return new ProjectDetailsForm(driverId);
		}

		public CmtLoginForm AtCmtLoginForm()
		{
			return new CmtLoginForm(driverId);
		}

		public SubmittedProjectForm AtSubmittedProjectForm()
		{
			return new SubmittedProjectForm(driverId);
		}

		public ProjectOverviewForm AtProjectOverviewForm()
		{
			return new ProjectOverviewForm(driverId);
		}

		public UsersForm AtUsersForm()
		{
			return new UsersForm(driverId);
		}

		public TenantsConfigurationForm AtTenantsConfigurationForm()
		{
			return new TenantsConfigurationForm(driverId);
		}

	    public ProfilesOverviewForm AtProfilesOverviewForm()
	    {
            return new ProfilesOverviewForm(driverId);
	    }

	    public ProfileNameForm AtProfileNameForm()
	    {
            return new ProfileNameForm(driverId);
	    }

	    public ProfileOutlookConfigForm AtProfileOutlookConfigForm()
	    {
            return new ProfileOutlookConfigForm(driverId);
	    }

	    public ProfileMailboxUpdateForm AtProfileMailboxUpdateForm()
	    {
            return new ProfileMailboxUpdateForm(driverId);
	    }

	    public ProfileCreateUsersForm AtProfileCreateUsersForm()
	    {
            return new ProfileCreateUsersForm(driverId);
	    }

	    public ProfileCreateDistributionGroupsForm AtProfileCreateDistributionGroupsForm()
	    {
            return new ProfileCreateDistributionGroupsForm(driverId);
	    }

	    public ProfileSyncDistributionGroupsForm AtProfileSyncDistributionGroupsForm()
	    {
            return new ProfileSyncDistributionGroupsForm(driverId);
	    }

        public ProfileContentToMigrateForm AtProfileContentToMigrateForm => new ProfileContentToMigrateForm(driverId);
        public ProfileFilterMessagesForm AtProfileFilterMessagesForm => new ProfileFilterMessagesForm(driverId);
        public ProfileFilterCalendarForm AtProfileFilterCalendarForm => new ProfileFilterCalendarForm(driverId);
        public ProfileFilterTasksForm AtProfileFilterTasksForm => new ProfileFilterTasksForm(driverId);
        public ProfileFilterNoteForm AtProfileFilterNoteForm => new ProfileFilterNoteForm(driverId);
        public ProfileLargeItemsHandleForm AtProfileLargeItemsHandleForm => new ProfileLargeItemsHandleForm(driverId);

        public ProfileTranslateSourceEmailForm AtProfileTranslateSourceEmailForm => new ProfileTranslateSourceEmailForm(driverId);
        public ProfileTypeOfMailboxContentForm AtProfileTypeOfMailboxContentForm => new ProfileTypeOfMailboxContentForm(driverId);
        public ProfileHandleBadItemsForm AtProfileHandleBadItemsForm => new ProfileHandleBadItemsForm(driverId);
        public ProfileHandleFoldersForm AtProfileHandleFoldersForm => new ProfileHandleFoldersForm(driverId);
        public ProfileToLicenseMailboxesForm AtProfileToLicenseMailboxesForm => new ProfileToLicenseMailboxesForm(driverId);
      


    }
}