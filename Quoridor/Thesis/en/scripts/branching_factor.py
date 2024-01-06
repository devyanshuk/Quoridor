import concurrent.futures, argparse, subprocess
import matplotlib.pyplot as plt

def run_subprocess(dim, s1, s2, simulations):
    cmd = [
        "dotnet",
        "run",
        "play",
        f"-s1={s1}",
        "-depth=1",
        f"-s2={s2}",
        "-b",
        "-verbose=false",
        f"-numsim={simulations}",
        f"-dimension={dim}"
    ]

    result = subprocess.run(cmd, cwd="../../../Quoridor.ConsoleApp/", capture_output=True)
    branching_factor = float(result.stdout.splitlines()[-1].split()[-1].decode("utf-8"))
    return dim, branching_factor

def save_fig(dim_map):
    xs = sorted(dim_map)
    ys = [dim_map[i] for i in xs]
    min_bfs = [1]*len(xs)
    max_bfs = [(5 + 2 * (i - 1)**2) for i in xs]
    plt.xticks(xs)
    plt.xlim(min(xs), max(xs))
    plt.xlabel("Board dimension")
    plt.ylabel("Branching factor")
    plt.plot(xs, ys, marker='o', label="average")
    plt.plot(xs, min_bfs, label="minimum")
    plt.plot(xs, max_bfs, marker='x', label="maximum")
    plt.legend()
    plt.grid()
    plt.savefig("../../img/branching_factor.png")
    plt.show()

def main(args):
    dims_to_examine = [3, 5, 7, 9]
    nums = len(dims_to_examine)

    dim_map = {}

    with concurrent.futures.ProcessPoolExecutor() as executor:
        result = executor.map(run_subprocess, dims_to_examine, [args.s1]*nums, [args.s2]*nums, [args.simulations]*nums)

        for dim, branching_factor in result:
            dim_map[dim] = branching_factor
            print(f"{dim}x{dim} board: {branching_factor}")

    print(dim_map)

    save_fig(dim_map)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--s1', type=str, default="minimaxab" ,help='Strategy for player 1')
    parser.add_argument('--s2', type=str, default="semirandom", help='Strategy for player 2')
    parser.add_argument('--simulations', type=int, default=1000, help='Number of simulations')
    args = parser.parse_args()
    #{3: 8.897959, 5: 17.214174, 7: 36.948112, 9: 62.93257}

    main(args)