import type { User, Role } from '../types';

/**
 * JWTトークンの内容をデコードしてペイロードを取得する
 * @param token JWTトークン
 * @returns デコードされたペイロードオブジェクト
 */
export const decodeToken = (token: string): any => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      window.atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error('Failed to decode JWT token:', error);
    return null;
  }
};

/**
 * JWTのペイロードをUserオブジェクトにマッピングする
 * @param payload デコードされたJWTペイロード
 * @returns Userオブジェクト
 */
export const mapPayloadToUser = (payload: any): User | null => {
  if (!payload) return null;

  // .NETのClaimタイプマッピングに対応
  // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier -> id
  // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress -> email
  // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name -> name
  // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role -> role

  const id = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.sub;
  const email = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || payload.email;
  const name = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || payload.unique_name;
  
  // Role can be in multiple claim types depending on the generator
  const role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
               payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'] || 
               payload.role;

  console.log('Mapped user from payload:', { id, email, name, role });

  return {
    id: parseInt(id, 10),
    email: email,
    name: name,
    role: role as Role,
  };
};
