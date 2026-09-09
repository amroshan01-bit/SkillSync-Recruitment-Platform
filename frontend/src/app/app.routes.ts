import { Routes } from '@angular/router';

import { EmployerProfileComponent } from './features/employer/employer-profile/employer-profile.component';
import { VacancyManagementComponent } from './features/employer/vacancy-management/vacancy-management.component';
import { JobSeekerProfileComponent } from './features/seeker/job-seeker-profile/job-seeker-profile.component';
import { JobMatchesComponent } from './features/seeker/job-matches/job-matches.component';
import { MyApplicationsComponent } from './features/seeker/my-applications/my-applications.component';
import { ManageSkillsComponent } from './features/seeker/manage-skills/manage-skills.component';

export const routes: Routes = [
  {
    path: 'seeker/profile',
    component: JobSeekerProfileComponent,
  },
  {
    path: 'seeker/matches',
    component: JobMatchesComponent,
  },
  {
    path: 'seeker/applications',
    component: MyApplicationsComponent,
  },
  {
    path: 'seeker/skills',
    component: ManageSkillsComponent,
  },
  {
    path: 'employer/profile',
    component: EmployerProfileComponent,
  },
  {
    path: 'employer/vacancies',
    component: VacancyManagementComponent,
  },
  {
    path: '',
    redirectTo: 'employer/profile',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'employer/profile',
  },
];
