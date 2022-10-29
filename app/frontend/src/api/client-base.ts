export class ClientBase {
  protected transformOptions(options: RequestInit) {
    const token = localStorage.getItem('token'); // TODO: берём access token откуда-нибудь
    options.headers = {
      ...options.headers,
      Authorization: 'Bearer' + token,
    }

    return Promise.resolve(options);
  }
}
