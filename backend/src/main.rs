use warp::Filter;

const PORT: u16 = 3030;
const HOST: [u8; 4] = [127, 0, 0, 1];

#[tokio::main]
async fn main() {
    // GET /hello/warp => 200 OK with body "Hello, warp!"
    let hello = warp::path!("hello" / String).map(|name| format!("Hello, {}!", name));

    warp::serve(hello).run((HOST, PORT)).await;
}
