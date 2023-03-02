namespace CRUD_test {
    public class Player {

        public int id { get; set; }
        public string playerName { get; set; }
        public int playerNumber { get; set; }
        public int TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
