export interface EmployerProfile {
  id: string;
  userId: string;
  companyName: string;
  industry: string;
  companySize: string;
  website: string | null;
  location: string;
  aboutCompany: string;
  logoPath: string | null;
  contactPerson: string;
  emailAddress: string;
  phoneNumber: string;
  createdAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateEmployerProfile {
  userId: string;
  companyName: string;
  industry: string;
  companySize: string;
  website: string | null;
  location: string;
  aboutCompany: string;
  logoPath: string | null;
  contactPerson: string;
  emailAddress: string;
  phoneNumber: string;
}

export interface UpdateEmployerProfile {
  companyName: string;
  industry: string;
  companySize: string;
  website: string | null;
  location: string;
  aboutCompany: string;
  logoPath: string | null;
  contactPerson: string;
  emailAddress: string;
  phoneNumber: string;
}