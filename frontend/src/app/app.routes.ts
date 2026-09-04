import { Routes } from '@angular/router';

import { JobSeekerProfileComponent } from './features/seeker/job-seeker-profile/job-seeker-profile.component';

export const routes: Routes = [
  {
    path: 'seeker/profile',
    component: JobSeekerProfileComponent,
  },
  {
    path: '',
    redirectTo: 'seeker/profile',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'seeker/profile',
  },
];