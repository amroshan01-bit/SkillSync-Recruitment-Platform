import { Routes } from '@angular/router';

import { EmployerProfileComponent } from './features/employer/employer-profile/employer-profile.component';
import { VacancyManagementComponent } from './features/employer/vacancy-management/vacancy-management.component';
import { JobSeekerProfileComponent } from './features/seeker/job-seeker-profile/job-seeker-profile.component';

export const routes: Routes = [
  {
    path: 'seeker/profile',
    component: JobSeekerProfileComponent,
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