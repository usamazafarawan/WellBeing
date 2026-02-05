import { API_BASE_URL } from './api';

const USER_ID_KEY = 'wellbeing_user_id';
const USER_CLIENT_KEY = 'wellbeing_user_client_id';

export async function getOrCreateUserId(clientId: number): Promise<string> {
  if (typeof window === 'undefined') {
    return '00000000-0000-0000-0000-000000000000';
  }

  const storedClientId = localStorage.getItem(USER_CLIENT_KEY);
  let userId = localStorage.getItem(USER_ID_KEY);

  if (userId && storedClientId === clientId.toString()) {
    try {
      const response = await fetch(`${API_BASE_URL}/AspNetUsers/${userId}`, {
        cache: 'no-store',
        headers: {
          'Accept': 'application/json',
        },
      });

      if (response.ok) {
        return userId;
      }
    } catch (error) {
      console.log('User verification failed, will create new user');
    }
  }

  try {
    const newUserId = await createAnonymousUser(clientId);
    localStorage.setItem(USER_ID_KEY, newUserId);
    localStorage.setItem(USER_CLIENT_KEY, clientId.toString());
    return newUserId;
  } catch (error) {
    console.error('Failed to create user:', error);
    const fallbackId = generateGuid();
    localStorage.setItem(USER_ID_KEY, fallbackId);
    localStorage.setItem(USER_CLIENT_KEY, clientId.toString());
    throw new Error('Failed to create user. Please try again.');
  }
}

async function createAnonymousUser(clientId: number): Promise<string> {
  const timestamp = Date.now();
  const randomStr = Math.random().toString(36).substring(2, 8);
  const userName = `anonymous_${timestamp}_${randomStr}`;
  const email = `${userName}@survey.local`;
  const placeholderHash = 'ANONYMOUS_USER_NO_PASSWORD_' + timestamp;

  const userData = {
    FirstName: 'Anonymous',
    LastName: 'User',
    IsFirstLogin: true,
    RequiresOtpVerification: false,
    AuthMethod: 'anonymous',
    ClientsId: clientId,
    UserName: userName,
    Email: email,
    PasswordHash: placeholderHash,
    PhoneNumber: null,
    LeadershipLevel: null,
    Tenant: null,
  };

  const response = await fetch(`${API_BASE_URL}/AspNetUsers`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    },
    body: JSON.stringify(userData),
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create user: ${response.status} ${errorText}`);
  }

  const user = await response.json();
  return user.id;
}

export function getUserId(): string {
  if (typeof window === 'undefined') {
    return '00000000-0000-0000-0000-000000000000';
  }

  const userId = localStorage.getItem(USER_ID_KEY);
  return userId || '00000000-0000-0000-0000-000000000000';
}

export function setUserId(userId: string): void {
  if (typeof window !== 'undefined') {
    localStorage.setItem(USER_ID_KEY, userId);
  }
}

function generateGuid(): string {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
    const r = (Math.random() * 16) | 0;
    const v = c === 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}
