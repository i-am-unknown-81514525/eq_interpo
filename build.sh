rm -r build || true
msbuild || true
cd ui/stdin_handler
cargo build --release || true
cd ../..
mkdir build
cp eq_interpo/bin/Debug/* build || true
cp ui/bin/Debug/* build || true
cp ui/stdin_handler/target/release/* build || true
touch build/read-log
