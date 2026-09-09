export interface JobApplication {
  id: string;
  vacancyId: string;
  jobSeekerUserId: string;
  jobTitle: string;
  companyName: string;
  status: string;
  coverLetter: string | null;
  appliedAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateJobApplicationRequest {
  vacancyId: string;
  jobSeekerUserId: string;
  coverLetter: string;
}

export interface UpdateApplicationStatusRequest {
  status: string;
}